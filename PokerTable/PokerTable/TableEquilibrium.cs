using Microsoft.SolverFoundation.Services;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace PokerTable
{
    internal class TableEquilibrium
    {
        private readonly int[] _chipsSet;
        private readonly int referChip;
        internal int MinEquilibriumMoves { get; private set; }
        internal bool IsEquilibriumAble { get; private set; }
        


        internal TableEquilibrium(int[] chipsSet)
        {
            _chipsSet = chipsSet;

            CheckEquilibriumAble();

            if (IsEquilibriumAble)
            {
                referChip = (int)_chipsSet.Average();
            }
        }

        void CheckEquilibriumAble()
        {
            IsEquilibriumAble = _chipsSet.Average() % 1 == 0 && _chipsSet.Min() >= 0;

        }

        public void MinEquilibriumMovesCalc()
        {
            if (IsEquilibriumAble)
            {
                string strModel = @"Model[
                    Parameters[Sets,Source,Sink],
                    Parameters[Reals,Supply[Source],Demand[Sink],Cost[Source,Sink]],
                    Decisions[Reals[0,Infinity],flow[Source,Sink],TotalCost],
                 Constraints[
                 TotalCost == Sum[{i,Source},{j,Sink},Cost[i,j]*flow[i,j]],
                    Foreach[{i,Source}, Sum[{j,Sink},flow[i,j]]<=Supply[i]],
                    Foreach[{j,Sink}, Sum[{i,Source},flow[i,j]]>=Demand[j]]],
                     Goals[Minimize[TotalCost]] ]";

                // Load OML-Model
                SolverContext context = SolverContext.GetContext();
                context.LoadModel(FileFormat.OML, new StringReader(strModel));
                context.CurrentModel.Name = "Transportation Problem";

                // Supply table
                DataTable pSupply = new DataTable();
                pSupply.Columns.Add("SupplyNode", Type.GetType("System.String"));
                pSupply.Columns.Add("Supply", Type.GetType("System.Int32"));

                // Demand table
                DataTable pDemand = new DataTable();
                pDemand.Columns.Add("DemandNode", Type.GetType("System.String"));
                pDemand.Columns.Add("Demand", Type.GetType("System.Int32"));

                // OD-Matrix
                DataTable pCost = new DataTable();
                pCost.Columns.Add("SupplyNode", Type.GetType("System.String"));
                pCost.Columns.Add("DemandNode", Type.GetType("System.String"));
                pCost.Columns.Add("Cost", Type.GetType("System.Int32"));

                //// Fill tables
                //   Fill Supply and Demand
                for (int current = 0; current < _chipsSet.Length; current++)
                {
                    if (_chipsSet[current] > referChip)
                    {
                        pSupply.Rows.Add(current, _chipsSet[current] - referChip);
                        continue;
                    }
                    pDemand.Rows.Add(current, referChip - _chipsSet[current]);
                }

                // Fill Arcs (or OD-Matrix)

                for (int supplier = 0; supplier < _chipsSet.Length; supplier++)
                {
                    if (_chipsSet[supplier] <= referChip)
                    {
                        continue;
                    }

                    for (int demander = 0; demander < _chipsSet.Length; demander++)
                    {
                        if (_chipsSet[demander] > referChip)
                        {
                            continue;
                        }

                        var distLeft = Math.Max(demander, supplier) - Math.Min(supplier, demander);
                        var distRight = _chipsSet.Length - Math.Max(demander, supplier) + Math.Min(supplier, demander);
                        var arc = Math.Min(distLeft, distRight);
                        pCost.Rows.Add(supplier, demander, arc);
                    }

                }
                // Bind values from tables to parameter of the OML model

                foreach (Parameter p in context.CurrentModel.Parameters)
                {
                    switch (p.Name)
                    {
                        case "Supply":
                            p.SetBinding(pSupply.AsEnumerable(), "Supply", "SupplyNode");
                            break;
                        case "Demand":
                            p.SetBinding(pDemand.AsEnumerable(), "Demand", "DemandNode");
                            break;
                        case "Cost":
                            p.SetBinding(pCost.AsEnumerable(), "Cost", "SupplyNode", "DemandNode");
                            break;
                    }
                }

                Solution solution = context.Solve();

                // Fetch results: minimized total costs
                foreach (Decision desc in solution.Decisions)
                {
                    if (desc.Name == "TotalCost")
                    {
                        foreach (object[] value in desc.GetValues())
                        {
                            MinEquilibriumMoves = Convert.ToInt32(value[0]);
                        }
                    }
                }
            }
        }
    }
}

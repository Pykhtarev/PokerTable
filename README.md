# Poker Table Equalizing
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Description of the problem

 Jose set up a circular poker table for his friends so that each of the seats at the table has the same number of poker chips.
 But when Jose wasn’t looking, someone rearranged all of the chips so that they are no longer evenly distributed!
 Now Jose needs to redistribute the chips so that every seat has the same number before his friends arrive.
 But Jose is very meticulous: to ensure that he doesn’t lose any chips in the process, he only moves chips between adjacent seats.
 Moreover, he only moves chips one at a time. What is the minimum number of chip moves
 Jose will need to make to bring the chips back to equilibrium?

`Input:`

>[1, 5, 9, 10, 5]

`Expected Output:`

12

`Input:`

>[1, 2, 3]

`Expected Output:`

1


`Input:`

>[0, 1, 1, 1, 1, 1, 1, 1, 1, 2]

`Expected Output:`

1


## Solution

In this case I applied the solution for the transport problem based on OML model

## Tech 

- .NET Framework
- MSF for building OML model and computation result 

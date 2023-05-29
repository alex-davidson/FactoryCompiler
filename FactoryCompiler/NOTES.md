# FactoryCompiler Tech Notes

## Model

* A Region is connected to zero or more Networks by Transport Links.
* A Region contains zero or more Groups.

* A Group can contain one or more Groups, XOR a Production.
* A Group can be repeated. Default: 1

* A Production consists of a single Recipe and Factory Type, and a Count.

* A Network is connected to one or more Regions by Transport Links.

* A Transport Link consists of a Region, a Network, a Direction and an Item Type.

The idea is to break up a factory into manageable chunks (Regions) connected by Networks.
Within a Region it is ASSUMED that all item transport is perfectly efficient, ie. any excess
is first used to cover shortfalls elsewhere in the Region.

Each Transport Link moves one type of item in one direction. It's ASSUMED that a Region is
only ever a net producer or consumer of an item; behaviour is undefined if a Transport Link
is bidirectional.

## Summaries

Regions, Networks and Groups can all be *summarised* in terms of their net production or
consumption of resources.

* A Region summary only considers resources which are not imported/exported.
  * If a Region exports Iron Ingots and has an excess of these, then these are all accounted
    for by the Network they are exported to. The Region summary will not consider Iron Ingots.
  * However, if a Region *imports* Iron Ingots and has an excess of these, the summary will
    show the excess as there is no need to import.

* A Network summary considers the net volumes of the imports and exports it handles, totalled
  across all connected Regions.
  * Again, this only considers actual imports/exports. A Region with eg. a negative volume of
    an export is not considered here.
  * Since multiple networks may service multiple regions (many to many) determining net excess/
    shortfall can be rather complicated. We use an implementation of Two-Phase Simplex for this,
    because I just happened to have one lying around.

* A Group summary considers the Group's contribution to its parent Region's totals, taking into
    account *turnover*.
  * Group A and B are children of Region R. Elsewhere in R, 500 Iron Ingots are being produced.
    A consumes 200 and B consumes 800. This is a net shortfall of 500, but B is more of a 
    problem here than A. This is therefore summarised as a shortfall of 100 for A and 400 for B.

Area summaries are shown as pie charts adjacent to the area's label.
* Area of the pie is proportional to the logarithm of total turnover.
* Excess is green, shortfall is red, and remaining turnover is blank.

Transport Links can also be summarised in terms of the volumes of resources they carry.
* Area of the pie is proportional to the logarithm of total volume transported.
* Imports are yellow, exports are blue.

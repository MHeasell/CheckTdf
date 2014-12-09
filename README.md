CheckTdf
========

CheckTdf is a command line utility
that can scan Total Annihilation TDF format files
(.tdf, .ota, .fbi, .gui) for errors and potential mistakes.
It can also scan inside HPI archives,
processing all the TDF files it finds.

Usage
-----

    CheckTdf.exe <filename>

TdfCheck will output errors and warnings to standard output,
one line for each issue.
It also has some defined exit codes:

    1 --- a program error occurred.
    2 --- parsing errors were generated.
    4 --- warnings were generated.

The warning and parse error code can be generated in combination
via bitwise OR, so you may also see an exit code of `6`,
representing that both errors and warnings were found.

Requirements
------------

CheckTdf is written in C#
and requires .NET Framework version 4.0 or later.

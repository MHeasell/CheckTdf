CheckTdf
========

CheckTdf is a command line utility
that can scan Total Annihilation TDF format files
(.tdf, .ota, .fbi, .gui) for errors and potential mistakes.
It can also scan inside HPI archives,
processing all the TDF files it finds.

Requirements
------------

CheckTdf is written in C#
and requires .NET Framework version 4.0 or later.

Quick Start
-----------

If you are impatient or do not want to use the command line,
you can use the provided batch runner.
Find the TDF file or HPI archive you wish to check
and drag it onto `TdfCheckRunner.bat`.
Any generated errors and warnings will be written to
`CheckTdf-output.txt` in the CheckTdf directory.
If the file is blank, your file was error/warning free.

Command-Line Usage
------------------

You can also run the command-line program directly.

    CheckTdf.exe <filename>

TdfCheck will output found errors and warnings to standard output,
one line for each issue.
It also has some defined exit codes:

    1 --- a program error occurred.
    2 --- parsing errors were generated.
    4 --- warnings were generated.

The warning and parse error code can be generated in combination
via bitwise OR, so you may also see an exit code of `6`,
representing that both errors and warnings were found.

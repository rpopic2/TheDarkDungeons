#!/bin/bash
# auto dotnet test runner by rpopic2
# last edit 22 May 2022
DIR=$(dirname "$0")
echo $DIR
clear
echo Starting tests...
date
dotnet build $DIR -v=q --nologo
rm testlist 2> /dev/null
list=$(find .. -name \*Test.cs)
for testitem in $list; do
    tempname=$(basename $testitem .cs)
    echo $tempname >> testlist
    echo Found test file $tempname
done

EXITCODE=0
for testitem2 in `cat testlist`; do
    echo -------------- $testitem2 -------------- 
    dotnet test $DIR --nologo --verbosity=q --no-build --filter=$testitem2
    if [ $? != 0 ]; then
        EXITCODE=1
    fi
done
rm testlist 2> /dev/null
exit $EXITCODE

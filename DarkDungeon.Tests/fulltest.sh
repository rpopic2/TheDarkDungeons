#!/bin/bash
# auto dotnet test runner by rpopic2
# last edit 22 May 2022
echo Starting tests...
date
dotnet build -v=q --nologo
rm testlist 2> /dev/null
list=$(find .. -name \*Test.cs)
for testitem in $list; do
    tempname=$(basename $testitem .cs)
    echo $tempname >> testlist
    echo Found test file $tempname
done

for testitem2 in `cat testlist`; do
    echo -------------- $testitem2 -------------- 
    dotnet test --nologo --verbosity=q --no-build --filter=$testitem2
done
rm testlist 2> /dev/null

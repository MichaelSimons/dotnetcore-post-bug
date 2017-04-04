#!/bin/bash

START=$(date +%s)

for i in {1..1000}
do
	curl -s -w "%{time_total}\n" -o /dev/null -d "test data" http://localhost:8000/
done

END=$(date +%s)
DIFF=$(( $END - $START ))
echo "It took $DIFF seconds"
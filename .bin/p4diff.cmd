#!/bin/sh
[ $# -eq 7 ] && p4merge "$2" "$5"
[ $# -eq 2 ] && p4merge "$1" "$2"
#!/usr/bin/env bash

CMD=${1:-"bash"}

exec env $(cat conf.vars) $(cat conf.vars.local) ${CMD}

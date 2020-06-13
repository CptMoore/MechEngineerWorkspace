#!/bin/sh

set -ex

#git submodule deinit -f .
#git submodule sync
git submodule update --init --remote
git submodule foreach 'git checkout master || :'
git submodule foreach 'git checkout Experimental || :'
git submodule foreach git pull

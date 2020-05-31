#!/bin/sh

set -ex

git submodule update --init --recursive
git submodule foreach git pull

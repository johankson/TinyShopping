#!/bin/bash
autorest --input-file=http://192.168.119.131:5000/swagger/v1/swagger.json --csharp.namespace=TinyShopping.Core.Net --add-credentials --output-folder=./net --useDateTimeOffset

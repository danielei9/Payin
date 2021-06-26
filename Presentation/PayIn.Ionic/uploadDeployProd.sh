#!/bin/bash

ionic plugin remove com.payin.nfc
ionic plugin add ../Plugins/Nfc

ionic build android

# https://apps.ionic.io/app/7f93d6b4/deploy
ionic upload --note "$1" --deploy=production
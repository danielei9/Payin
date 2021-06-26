#!/bin/bash

ionic plugin remove com.payin.nfc
ionic plugin add ../Plugins/Nfc

ionic plugin remove com.payin.palago.smartband
ionic plugin add ../Plugins/com.payin.palago.smartband

ionic run android



#NO BORRAR: ionic plugin add https://github.com/crosswalk-project/cordova-plugin-crosswalk-webview.git
#NO BORRAR: config.xml    <preference name="xwalkVersion" value="19" />
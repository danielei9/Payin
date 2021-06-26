#!/bin/bash

npm install
bower install
#grunt serve to test on your computer

ionic platform remove android
ionic platform remove ios
ionic platform add android
#ionic platform add ios

#ionic plugin add cordova-plugin-customurlscheme --variable URL_SCHEME=payin --save
#ionic plugin add cordova-plugin-crosswalk-webview --save

ionic resources --clean-cache
#cp resources/icon.png platforms/android/res/drawable

# https://forum.ionicframework.com/t/does-ionic-supports-windows-phone-8/29233/9
# http://www.badpenguin.org/how-to-make-your-ionic-cordova-app-to-run-under-windows-phone-8-1-and-desktop

# http://www.imagemagick.org/script/binary-releases.php#windows
#mkdir resources/windows/icon
#convert "resources/icon.png" "resources/windows/icon/ApplicationIcon.png"
    # <icon src="resources/windows/icon/ApplicationIcon.png" width="99" height="99" />
    # <icon src="resources/windows/icon/windows/logo.png" width="150" height="150" />
    # <icon src="resources/windows/icon/smalllogo.png" width="30" height="30" />
    # <icon src="resources/windows/icon/storelogo.png" width="50" height="50" />
    # <icon src="resources/windows/icon/lumia630.png" width="106" height="106" />
    # <icon src="resources/windows/icon/Wide310x150Logo.scale-100.png" width="310" height="150" />
    # <icon src="resources/windows/icon/Square71x71Logo.scale-240.png" width="170" height="170" />
    # <icon src="resources/windows/icon/Wide310x150Logo.scale-240.png" width="744" height="360" />
    # <icon src="resources/windows/icon/Square150x150Logo.scale-240.png" width="360" height="360" />
    # <icon src="resources/windows/icon/Square71x71Logo.scale-100.png" width="71" height="71" />
    # <icon src="resources/windows/icon/Square44x44Logo.scale-100.png" width="44" height="44" />
    # <icon src="resources/windows/icon/StoreLogo.scale-240.png" width="120" height="120" />
    # <icon src="resources/windows/icon/Square70x70Logo.scale-100.png" width="70" height="70" />
    # <icon src="resources/windows/icon/Square310x310Logo.scale-100.png" width="310" height="310" />
    # <splash src="resources/windows/screen/SplashScreenImage.png" width="768" height="1280" />
    # <splash src="resources/windows/screen/SplashScreen.png" width="620" height="300" />
    # <splash src="resources/windows/screen/SplashScreenPhone.scale-240.png" width="1152" height="1920" />

#mkdir platforms plugins www create folders for cordova
#cordova plugin add org.apache.cordova.device ... add interesting plugins
#grunt build && cordova run     android to run app on your phone

ionic plugin add com-badrit-macaddress
ionic plugin add cordova-plugin-camera 
ionic plugin add cordova-plugin-compat 
#ionic plugin add cordova-plugin-crosswalk-webview 
ionic plugin add cordova-plugin-datepicker 
ionic plugin add cordova-plugin-device 
ionic plugin add cordova-plugin-dialogs 
ionic plugin add cordova-plugin-network-information 
ionic plugin add cordova-plugin-sim 
ionic plugin add cordova-plugin-splashscreen 
ionic plugin add cordova-plugin-whitelist 
ionic plugin add cordova-sqlite-storage 
ionic plugin add cordova.plugins.diagnostic 
ionic plugin add ionic-plugin-keyboard
ionic plugin add phonegap-plugin-barcodescanner
#ionic plugin add phonegap-plugin-push

#ionic plugin add com-badrit-macaddress

#ionic plugin add com-badrit-macaddress
#ionic plugin add cordova-plugin-camera 
#ionic plugin add cordova-plugin-compat 
#ionic plugin add cordova-plugin-crosswalk-webview 
#ionic plugin add cordova-plugin-datepicker 
#ionic plugin add cordova-plugin-device 
#ionic plugin add cordova-plugin-dialogs 
#ionic plugin add cordova-plugin-network-information 
#ionic plugin add cordova-plugin-sim 
#ionic plugin add cordova-plugin-splashscreen 
#ionic plugin add cordova-plugin-whitelist 
#ionic plugin add cordova-sqlite-storage 
#ionic plugin add cordova.plugins.diagnostic 
#ionic plugin add ionic-plugin-keyboard
#ionic plugin add phonegap-plugin-barcodescanner
#ionic plugin add phonegap-plugin-push

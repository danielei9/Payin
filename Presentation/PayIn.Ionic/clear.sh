#!/bin/bash

# clear.sh
#tr -d '\r' < clear.sh > clear.fix
#mv clear.fix clear.sh
# plugin.sh
tr -d '\r' < plugin.sh > plugin.fix
mv plugin.fix plugin.sh
# initialize.sh
tr -d '\r' < initialize.sh > initialize.fix
mv initialize.fix initialize.sh
# generateAndroid.sh
tr -d '\r' < generateAndroid.sh > generateAndroid.fix
mv generateAndroid.fix generateAndroid.sh
# generateIos.sh
tr -d '\r' < generateIos.sh > generateIos.fix
mv generateIos.fix generateIos.sh
# hooks/after_plugin_rm/010_deregister_plugin.js
tr -d '\r' < hooks/after_plugin_rm/010_deregister_plugin.js > hooks/after_plugin_rm/010_deregister_plugin.fix
mv hooks/after_plugin_rm/010_deregister_plugin.fix hooks/after_plugin_rm/010_deregister_plugin.js
# hooks/after_plugin_add/010_register_plugin.js
tr -d '\r' < hooks/after_plugin_add/010_register_plugin.js > hooks/after_plugin_add/010_register_plugin.fix
mv hooks/after_plugin_add/010_register_plugin.fix hooks/after_plugin_add/010_register_plugin.js
# hooks/before_platform_add/init_directories.js
tr -d '\r' < hooks/before_platform_add/init_directories.js > hooks/before_platform_add/init_directories.fix
mv hooks/before_platform_add/init_directories.fix hooks/before_platform_add/init_directories.js
# hooks/after_prepare/010_add_platform_class.js
tr -d '\r' < hooks/after_prepare/010_add_platform_class.js > hooks/after_prepare/010_add_platform_class.fix
mv hooks/after_prepare/010_add_platform_class.fix hooks/after_prepare/010_add_platform_class.js
# hooks/after_prepare/020_remove_sass_from_platforms.js
tr -d '\r' < hooks/after_prepare/020_remove_sass_from_platforms.js > hooks/after_prepare/020_remove_sass_from_platforms.fix
mv hooks/after_prepare/020_remove_sass_from_platforms.fix hooks/after_prepare/020_remove_sass_from_platforms.js
# hooks/after_platform_add/010_install_plugins.js
tr -d '\r' < hooks/after_platform_add/010_install_plugins.js > hooks/after_platform_add/010_install_plugins.fix
mv hooks/after_platform_add/010_install_plugins.fix hooks/after_platform_add/010_install_plugins.js

ionic platforms remove android
ionic platforms remove ios
#rm -rf platforms
rm -R platforms

rm -R plugins

rm -R resources/android
#rm -R resources/ios

chmod 777 *
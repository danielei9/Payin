#!/usr/bin/env node

/**
 * After prepare, files are copied to the platforms/ios and platforms/android folders.
 * Lets clean up some of those files that arent needed with this hook.
 */
var fs = require('fs');
var path = require('path');

/**
 * Sets an override value to force the cordova build process to use the actual,
 * specified-in-the-config.xml versionCode.
 */
// https://issues.apache.org/jira/browse/CB-8976?jql=text%20~%20%22versionCode%22
module.exports = function forceAndroidVersionCode(context) {
  var et = context.requireCordovaModule('elementtree'),
      manifestPath = path.join(context.opts.projectRoot, 'platforms', 'android', 'AndroidManifest.xml'),
      manifestContents = fs.readFileSync(manifestPath, { encoding: 'utf8' }),
      manifest = et.parse(manifestContents),
      versionCode = manifest.getroot().get('android:versionCode'),
      buildExtrasPath = path.join(context.opts.projectRoot, 'platforms', 'android', 'build-extras.gradle');

  fs.writeFileSync(buildExtrasPath, "ext.cdvVersionCode=" + versionCode, { encoding: 'utf8' });
};

var deleteFolderRecursive = function(removePath) {
  if( fs.existsSync(removePath) ) {
    fs.readdirSync(removePath).forEach(function(file,index){
      var curPath = path.join(removePath, file);
      if(fs.lstatSync(curPath).isDirectory()) { // recurse
        deleteFolderRecursive(curPath);
      } else { // delete file
        fs.unlinkSync(curPath);
      }
    });
    fs.rmdirSync(removePath);
  }
};

var iosPlatformsDir = path.resolve(__dirname, '../../platforms/ios/www/lib/ionic/scss');
var androidPlatformsDir = path.resolve(__dirname, '../../platforms/android/assets/www/lib/ionic/scss');

deleteFolderRecursive(iosPlatformsDir);
deleteFolderRecursive(androidPlatformsDir);

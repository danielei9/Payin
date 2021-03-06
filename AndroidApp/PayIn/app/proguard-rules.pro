# Add project specific ProGuard rules here.
# By default, the flags in this file are appended to flags specified
# in /Users/ignacioballester/Library/Android/sdk/tools/proguard/proguard-android.txt
# You can edit the include path and order by changing the proguardFiles
# directive in build.gradle.
#
# For more details, see
#   http://developer.android.com/guide/developing/tools/proguard.html

# Add any project specific keep options here:

# If your project uses WebView with JS, uncomment the following
# and specify the fully qualified class name to the JavaScript interface
# class:
#-keepclassmembers class fqcn.of.javascript.interface.for.webview {
#   public *;
#}

-keepattributes Signature
-keep class sun.misc.Unsafe { *; }
-keep class com.google.gson.stream.** { *; }

-keep public class com.mobile.application.payin.dto.* { *; }

-keep public class com.mobile.application.payin.dto.results.** { *; }
-keep public class com.mobile.application.payin.dto.arguments.** { *; }

-verbose

-keep public class com.google.android.gms.**
-dontwarn com.google.android.gms.**

#-printseeds seeds.txt
#-printmapping mapping.txt
#!/bin/bash

# No funciona pero lo pongo para las características.
keytool -genkey -dname "CN=Xavier Jorge Cerdá, OU=Payment Innovation Network SL, O=Payin, L=Valencia, ST=Valencia, C=ES"  \
    -alias mobile01 \
    -keystore www/key/keystore.bks -keyalg RSA -keysize 2048 -validity 365 -storetype BKS \
    -providerClass org.bouncycastle.jce.provider.BouncyCastleProvider -providerpath bcprov-jdk15on-154.jar

# http://portecle.sourceforge.net/export-entry.html
# https://www.sslsupportdesk.com/portecle-advanced-keystore-creation-and-manipulation-tool/

package com.nxp.ltsm.mymifareapp.cards;

import java.io.Serializable;
import java.lang.reflect.Array;
import java.util.ArrayList;

import com.nxp.ltsm.mymifareapp.utils.Parsers;

import android.graphics.drawable.Drawable;

public class VirtualCard implements Serializable {
	private static final long serialVersionUID = 1L;
	
	public static final int VC_MIFARE_CLASSIC = 1;
	public static final int VC_MIFARE_DESFIRE = 2;
	
	private int		vcType;
	private int 	vcId;
	private String 	vcName;
	private String 	vcUID;
	private int vcIconRes;
	private String vcIconPath;
	private ArrayList<String> vcPublicMdac = new ArrayList<String>();
	
	public VirtualCard(int vctype, int vcId, String vcName, String vcUID, int vcIconRes, String vcIconPath) {
		this.vcType = vctype;
		this.vcId = vcId;
		this.vcName = vcName;
		this.vcUID = vcUID;
		this.vcIconRes = vcIconRes;
		this.vcIconPath = vcIconPath;
		
		vcPublicMdac.clear();
	}
	
	public int getVcType() {
		return vcType;
	}

	public void setVcType(int vcType) {
		this.vcType = vcType;
	}

	public String getVcIconPath() {
		return vcIconPath;
	}

	public void setVcIconPath(String vcIconPath) {
		this.vcIconPath = vcIconPath;
	}

	public int getVcId() {
		return vcId;
	}
	public static long getSerialversionuid() {
		return serialVersionUID;
	}

	public void setVcId(int vcId) {
		this.vcId = vcId;
	}
	
	public String getVcName() {
		return vcName;
	}
	public void setVcName(String vcName) {
		this.vcName = vcName;
	}
	
	public String getVcUID() {
		return vcUID;
	}
	public void setVcUID(String vcUID) {
		this.vcUID = vcUID;
	}
	
	public int getVcIconRes() {
		return vcIconRes;
	}
	public void setVcIconRes(int vcIconRes) {
		this.vcIconRes = vcIconRes;
	}
	
	public ArrayList<String> getVcPublicMdac() {
		return vcPublicMdac;
	}
	
	public void getVcPublicMdac(ArrayList<String> vcPublicMdac) {
		this.vcPublicMdac = vcPublicMdac;
	}
	
	
	public void addVcPublicMdacClassic(String newPublicMdac) {
		byte[] data = Parsers.hexToArray(newPublicMdac.substring(newPublicMdac.indexOf("D5"), newPublicMdac.indexOf("D5") + 10));
		int newSector = (int) data[2];
		
		// Remove the previous entry in case the MDAC is updated
		for(String publicMdac : vcPublicMdac) {
			data = Parsers.hexToArray(publicMdac.substring(publicMdac.indexOf("D5"), publicMdac.indexOf("D5") + 10));
			int sector = (int) data[2];
			
			if(sector == newSector) {
				vcPublicMdac.remove(publicMdac);
				break;
			}
		}
		
		vcPublicMdac.add(newPublicMdac);
	}
	
	// TODO: Update this method to remove previous MDACs for this particular AIDs
	public void addVcPublicMdacDF(String newPublicMdac) {
		vcPublicMdac.add(newPublicMdac);
	}
}

package com.nxp.ltsm.mymifareapp.cards;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.ArrayList;

import android.content.Context;
import android.util.Log;

public class VCEntries {
	private static ArrayList<VirtualCard> vcEntries = new ArrayList<VirtualCard>();
	
	public static void addEntry(VirtualCard vc, Context mContext) throws Exception {
		vcEntries.add(vc);
		
		// Store Virtual Card Entries
		VCEntries.SaveArrayListToSD(mContext, "VCPayinFile");
	}
	
	public static void deleteEntry(int vcEntryId, Context mContext) throws Exception {
		for(VirtualCard vc : vcEntries) {
			if(vc.getVcId() == vcEntryId) {
				vcEntries.remove(vc);
				
				break;
			}
		}
		
		// Store Virtual Card Entries
		VCEntries.SaveArrayListToSD(mContext, "VCPayinFile");
	}
	
	public static ArrayList<VirtualCard> getEntries() {
		return vcEntries;
	}
	
	public static void addVCEntryPublicMdac(int vcEntry, String mdac, Context mContext) throws Exception {
		for(VirtualCard v : vcEntries) {
			if(v.getVcId() == vcEntry) {
				if(v.getVcType() == VirtualCard.VC_MIFARE_CLASSIC)
					v.addVcPublicMdacClassic(mdac);
				else if(v.getVcType() == VirtualCard.VC_MIFARE_DESFIRE) {
					v.addVcPublicMdacDF(mdac);
				}
					
				break;
			}
		}
		
		// Save info
		VCEntries.SaveArrayListToSD(mContext, "VCPayinFile");
	}
	
	public static ArrayList<String> getVCEntryPublicMdac(int vcEntry) {
		for(VirtualCard v : vcEntries) {
			if(v.getVcId() == vcEntry) {
				return v.getVcPublicMdac();
			}
		}
		
		return null;
	}
	
    public static VirtualCard getVCEntry(int vcEntry) {
		for(VirtualCard v : vcEntries) {
			if(v.getVcId() == vcEntry) {
				return v;
			}
		}
		
		return null;
	}
    
	public static String getVCEntryUid(int vcEntry) {
		for(VirtualCard v : vcEntries) {
			if(v.getVcId() == vcEntry) {
				return v.getVcUID();
			}
		}
		
		return null;
	}
	
	public static void SaveArrayListToSD(Context mContext, String filename) throws Exception {
        try {
            String mensaje = "";
            for(VirtualCard v : vcEntries) {
                mensaje += "{" +
                    "'vcName':'" + v.getVcName() + "'," + 
                    "'vcType':'" + v.getVcType() + "'," + 
                    "'vcUID':'" + v.getVcUID() + "'" + 
                "},";
            }
            Log.d("VCEntries", "SaveArrayListToSD: start [" + mensaje + "]");
            
            FileOutputStream fos = mContext.openFileOutput(filename + ".dat", Context.MODE_PRIVATE);
            ObjectOutputStream oos = new ObjectOutputStream(fos);
            oos.writeObject(vcEntries);
            fos.close();
            
            Log.d("VCEntries", "SaveArrayListToSD: end");
        } catch (Exception e) {
            e.printStackTrace();
            throw e;
        }
    }
	
	@SuppressWarnings("unchecked")
	public static void ReadArrayListFromSD(Context mContext, String filename) throws Exception {
        try {
            Log.d("VCEntries", "ReadArrayListFromSD: start");
            
            FileInputStream fis = mContext.openFileInput(filename + ".dat");
            ObjectInputStream ois = new ObjectInputStream(fis);
            Object obj = (Object) ois.readObject();
            fis.close();
            
            vcEntries = (ArrayList<VirtualCard>) obj;
            
            String mensaje = "";
            for(VirtualCard v : vcEntries) {
                mensaje += "{" +
                    "'vcName':'" + v.getVcName() + "'," + 
                    "'vcType':'" + v.getVcType() + "'," + 
                    "'vcUID':'" + v.getVcUID() + "'" + 
                "},";
            }
            Log.d("VCEntries", "ReadArrayListFromSD: end [" + mensaje + "]");
		} catch (FileNotFoundException e) {
			e.printStackTrace();
        } catch (Exception e) {
            e.printStackTrace();
            throw e;
        }
    }
}

package com.mesqr.android;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;

import org.json.JSONArray;
import org.json.JSONObject;

import android.content.Context;
import android.database.sqlite.SQLiteDatabase;
import android.location.Location;

import com.mesqr.android.RestClient.RequestMethod;
import com.mesqr.android.db.DaoMaster;
import com.mesqr.android.db.DaoMaster.DevOpenHelper;
import com.mesqr.android.db.DaoSession;
import com.mesqr.android.db.Msq;
import com.mesqr.android.db.MsqDao;
import com.mesqr.android.db.TableDao;
import com.mesqr.android.model.MsqModel;
import com.mesqr.android.model.TableModel;

public class API {	
    private SQLiteDatabase db;

    private DaoMaster daoMaster;
    private DaoSession daoSession;
    private MsqDao msqDao;
    private TableDao tableDao;
    
    private static final String baseURL = "https://mesqr.azurewebsites.net/api/";
    
    private String SessionKey = null;
    
    //private Cursor cursor;
    
    public API(Context c)
    {
        DevOpenHelper helper = new DaoMaster.DevOpenHelper(c, "com.mesqr.android.db", null);
        db = helper.getWritableDatabase();
        daoMaster = new DaoMaster(db);
        daoSession = daoMaster.newSession();
        msqDao = daoSession.getMsqDao();
        tableDao = daoSession.getTableDao();
    }
    
    private String getSessionKey()
    {
    	if (SessionKey == null)
    		Login();
    	
    	return SessionKey;
    }
    
    private boolean Login()
    {
    	RestClient client = new RestClient(baseURL + "login");
    	client.AddParam("user", "tkglaser");
    	client.AddParam("password", "password");
    	try {
			client.Execute(RequestMethod.POST);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
    	
    	if (client.isOk())
    	{
    		SessionKey = client.response.replace('"', ' ').replace("\n", " ").trim();
    	}
    	return client.isOk();
    }
    
    public void cacheTable(String TableUID)
    {
		try {
			RestClient client = new RestClient(baseURL + "table");
			client.AddParam("id", TableUID);
			
			client.Execute(RequestMethod.GET);
			if (client.isOkAndHasString())
			{
				TableModel t = new TableModel(client.AsJsonObject());
			    if(tableDao.queryBuilder().where(TableDao.Properties.RowGuid.eq(t.getRowGuid())).list().size() == 0)
			    	tableDao.insert(t);
			}

		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
    }
    	
	public List<MsqModel> getNearbyMsqs(Location loc) {

		List<MsqModel> result = new ArrayList<MsqModel>();

		try {
			RestClient client = new RestClient(baseURL + "nearbymsq");
			client.AddParam("Latitude", loc.getLatitude());
			client.AddParam("Longitude", loc.getLongitude());
			client.AddParam("Accuracy", loc.getAccuracy());
			client.AddParam("Radius", 20000);
			client.AddParam("Skip", 0);
			client.AddParam("Take", 10);
			client.Execute(RestClient.RequestMethod.GET);
			
			if (client.isOkAndHasString()) {
				JSONArray msqs = client.AsJsonArray();
				
				for (int i = 0; i < msqs.length(); i++) {
				    JSONObject row = msqs.getJSONObject(i);
				    MsqModel msq = new MsqModel(row);
				    if(msqDao.queryBuilder().where(MsqDao.Properties.RowGuid.eq(msq.getRowGuid())).list().size() == 0)
				    	msqDao.insert(msq);
				    
				    if (msq.getTableUID() != "")
				    {
					    if (!msq.CanLinkTable())
					    	cacheTable(msq.getTableUID());
					    
					    msq.LinkTable();
				    }
				    
				    result.add(msq);
				}				
			}
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		return result;
		//return result.toArray(new Distance<Msq>[]{});
	}
	
	public List<MsqModel> getCachedMsqs()
	{
		List<MsqModel> result = new LinkedList<MsqModel>();
		for(Msq m : msqDao.queryBuilder().orderDesc(MsqDao.Properties.Entered).limit(10).list())
		{
			result.add(new MsqModel(m));
		}
		return result;
	}
	
	public MsqModel[] getMsqs() {		
		List<MsqModel> result = new ArrayList<MsqModel>();

		try {
			RestClient client = new RestClient(baseURL + "msq");
			client.Execute(RequestMethod.GET);
			
			if (client.isOkAndHasString()) {
				JSONArray msqs = client.AsJsonArray();
				
				for (int i = 0; i < msqs.length(); i++) {
				    JSONObject row = msqs.getJSONObject(i);
				    MsqModel msq = new MsqModel(row);
				    result.add(msq);
				}				
			}
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		return result.toArray(new MsqModel[]{});
	}
	
	public void SaveMsq(MsqModel m)
	{		
		try {
			RestClient client = new RestClient(baseURL + "msq");
			client.AddParam("Message", m.getMessage());
			client.AddParam("FriendlyPosition", m.getFriendlyPosition());
			client.AddParam("Latitude", m.getLatitude());
			client.AddParam("Longitude", m.getLongitude());
			client.AddParam("Accuracy", m.getAccuracy());
			client.AddParam("key", getSessionKey());
			client.Execute(RestClient.RequestMethod.POST);
			
			if (client.isOkAndHasString()) {
				MsqModel t = new MsqModel(client.AsJsonObject());
				
		    	msqDao.insert(t);
			}
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}	
}

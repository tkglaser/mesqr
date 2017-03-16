package com.mesqr.android.model;

import java.util.Date;

import org.json.JSONObject;

import android.location.Location;

import com.mesqr.android.db.Msq;
import com.mesqr.android.db.Table;
import com.mesqr.android.db.TableDao;
import com.mesqr.android.db.TableDao.Properties;

public class MsqModel extends Msq {
	
	public MsqModel(Location loc, String message)
	{
		Location = loc;
		setLatitude(loc.getLatitude());
		setLongitude(loc.getLongitude());
		setAccuracy(loc.getAccuracy());
		
		setMessage(message);
		setFriendlyPosition("not implemented");
	}
	
	public MsqModel(JSONObject json) {
		try {
			setMessage(json.getString("Message"));
			setFriendlyPosition(json.getString("FriendlyPosition"));
			setRowGuid(json.getString("ID"));
			setUserName(json.getString("UserName"));
			TableUID = json.optString("TableId");
			setLatitude(json.getDouble("Latitude"));
			setLongitude(json.getDouble("Longitude"));
			setAccuracy(json.getDouble("Accuracy"));
			setEntered(new Date());

			Location = new Location("API");
			Location.setLatitude(getLatitude());
			Location.setLongitude(getLongitude());
			Location.setAccuracy((float) getAccuracy());
			if (json.has("Altitude"))
			{
				setAltitude(json.getDouble("Altitude"));
				Location.setAltitude(getAltitude());
			}
		} catch (Exception e) {
			// TODO: handle exception
		}
	}
	
	public MsqModel(Msq m)
	{
		setMessage(m.getMessage());
		setFriendlyPosition(m.getFriendlyPosition());
		setRowGuid(m.getRowGuid());
		setUserName(m.getUserName());
		setTableId(m.getTableId());
		setLatitude(m.getLatitude());
		setLongitude(m.getLongitude());
		setAccuracy(m.getAccuracy());
		setEntered(m.getEntered());

		Location = new Location("API");
		Location.setLatitude(getLatitude());
		Location.setLongitude(getLongitude());
		Location.setAccuracy((float) getAccuracy());
		if (m.getAltitude() != null)
		{
			setAltitude(m.getAltitude());
			Location.setAltitude(getAltitude());
		}
	}
	
	private String TableUID;
	
	public String getTableUID()
	{
		return TableUID;
	}
	
	public boolean CanLinkTable()
	{
        TableDao targetDao = daoSession.getTableDao();
        return targetDao.queryBuilder().where(Properties.RowGuid.eq(TableUID)).count() == 1;		
	}
	
	public void LinkTable()
	{
        TableDao targetDao = daoSession.getTableDao();
        Table t = targetDao.queryBuilder().where(Properties.RowGuid.eq(TableUID)).unique();
        setTable(t);
	}

	private Location Location;
	
	public Location getLocation()
	{
		return Location;
	}
}

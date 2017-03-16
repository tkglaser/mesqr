package com.mesqr.android.model;

import java.util.Date;

import org.json.JSONObject;

import android.location.Location;

import com.mesqr.android.db.Table;

public class TableModel extends Table 
{
	public TableModel(JSONObject json) {
		try {
			setRowGuid(json.getString("ID"));
			setName(json.getString("Name"));
			setLatitude(json.getDouble("Latitude"));
			setLongitude(json.getDouble("Longitude"));
			setTableRadius(json.getDouble("TableRadius"));
			setEntered(new Date());
			
			Location = new Location("API");
			Location.setLatitude(getLatitude());
			Location.setLongitude(getLongitude());
			Location.setAccuracy(1);
		} catch (Exception e) {
			// TODO: handle exception
		}
	}
	
	private Location Location;
	
	public Location getLocation()
	{
		return Location;
	}
}

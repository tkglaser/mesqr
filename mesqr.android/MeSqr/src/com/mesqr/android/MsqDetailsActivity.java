package com.mesqr.android;

import android.content.Context;
import android.location.Location;
import android.location.LocationManager;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

import com.google.android.maps.MapActivity;
import com.google.android.maps.MapView;
import com.mesqr.android.model.MsqModel;

public class MsqDetailsActivity extends MapActivity {
	
    API api;    
	LocationManager mLocationManager;
	TextView tvMessage;
	Location mCurrentLocation = null;
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
	    super.onCreate(savedInstanceState);
	    api = new API(getApplicationContext());
	    setContentView(R.layout.msq_details);
	    MapView mapView = (MapView) findViewById(R.id.msq_details_mapview);
	    mapView.setBuiltInZoomControls(true);
	    mapView.displayZoomControls(true);

        mLocationManager =
                (LocationManager) this.getSystemService(Context.LOCATION_SERVICE);
        
        mCurrentLocation = mLocationManager.getLastKnownLocation(LocationManager.NETWORK_PROVIDER);
	    
	    tvMessage = (TextView)findViewById(R.id.editTextMessage);
	}
	
	public void Send(View v)
	{
    	api.SaveMsq(new MsqModel(mCurrentLocation, tvMessage.getText().toString()));
    	finishActivity(0);
    	finish();
	}

	@Override
	protected boolean isRouteDisplayed() {
		// TODO Auto-generated method stub
		return false;
	}

}

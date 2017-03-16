package com.mesqr.android;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.support.v4.app.FragmentTransaction;
import android.view.Menu;
import android.view.View;
import android.widget.ListView;
import android.widget.SimpleAdapter;
import android.widget.TextView;
import android.widget.Toast;

import com.actionbarsherlock.app.ActionBar;
import com.actionbarsherlock.app.ActionBar.Tab;
import com.actionbarsherlock.app.ActionBar.TabListener;
import com.actionbarsherlock.app.SherlockActivity;
import com.mesqr.android.model.MsqModel;


public class MainActivity extends SherlockActivity implements TabListener {

	private final LocationListener listener = new LocationListener() {

	    public void onLocationChanged(Location location) {
	    	mLocationStatus.setText("P: " 
	    			+ location.getLatitude() + ", " + location.getLongitude() + "A: " + location.getAccuracy());
	    	
	    	if (isBetterLocation(location, mCurrentLocation))
	    	{
	        	Toast.makeText(getBaseContext(), "New Location Received", Toast.LENGTH_SHORT).show();
	    		mCurrentLocation = location;
		    	Refresh(api.getNearbyMsqs(mCurrentLocation));
	    	}	    	
        }

		public void onProviderDisabled(String arg0) {
			// TODO Auto-generated method stub
			
		}

		public void onProviderEnabled(String arg0) {
			// TODO Auto-generated method stub
			
		}

		public void onStatusChanged(String arg0, int arg1, Bundle arg2) {
			// TODO Auto-generated method stub
			
		}
	    
	};
	
	private static final int TWO_MINUTES = 1000 * 60 * 2;

	/** Determines whether one Location reading is better than the current Location fix
	  * @param location  The new Location that you want to evaluate
	  * @param currentBestLocation  The current Location fix, to which you want to compare the new one
	  */
	protected boolean isBetterLocation(Location location, Location currentBestLocation) {
	    if (currentBestLocation == null) {
	        // A new location is always better than no location
	        return true;
	    }

	    // Check whether the new location fix is newer or older
	    long timeDelta = location.getTime() - currentBestLocation.getTime();
	    boolean isSignificantlyNewer = timeDelta > TWO_MINUTES;
	    boolean isSignificantlyOlder = timeDelta < -TWO_MINUTES;
	    boolean isNewer = timeDelta > 0;

	    // If it's been more than two minutes since the current location, use the new location
	    // because the user has likely moved
	    if (isSignificantlyNewer) {
	        return true;
	    // If the new location is more than two minutes older, it must be worse
	    } else if (isSignificantlyOlder) {
	        return false;
	    }

	    // Check whether the new location fix is more or less accurate
	    int accuracyDelta = (int) (location.getAccuracy() - currentBestLocation.getAccuracy());
	    boolean isLessAccurate = accuracyDelta > 0;
	    boolean isMoreAccurate = accuracyDelta < 0;
	    boolean isSignificantlyLessAccurate = accuracyDelta > 200;

	    // Check if the old and new location are from the same provider
	    boolean isFromSameProvider = isSameProvider(location.getProvider(),
	            currentBestLocation.getProvider());

	    // Determine location quality using a combination of timeliness and accuracy
	    if (isMoreAccurate) {
	        return true;
	    } else if (isNewer && !isLessAccurate) {
	        return true;
	    } else if (isNewer && !isSignificantlyLessAccurate && isFromSameProvider) {
	        return true;
	    }
	    return false;
	}

	/** Checks whether two providers are the same */
	private boolean isSameProvider(String provider1, String provider2) {
	    if (provider1 == null) {
	      return provider2 == null;
	    }
	    return provider1.equals(provider2);
	}

    private API api;    
	private LocationManager mLocationManager;
	private TextView mLocationStatus;
	private Location mCurrentLocation = null;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        final ActionBar ab = getSupportActionBar();
        
        ab.addTab(ab.newTab().setText("BOOM").setTabListener(this));
        
        api = new API(getApplicationContext());
        mLocationManager =
                (LocationManager) this.getSystemService(Context.LOCATION_SERVICE);
        
        mCurrentLocation = mLocationManager.getLastKnownLocation(LocationManager.NETWORK_PROVIDER);
        
        Refresh(api.getCachedMsqs());
                
    	mLocationManager.requestLocationUpdates(LocationManager.NETWORK_PROVIDER,
    	        1000,          // 1-second interval.
    	        10,             // 10 meters.
    	        listener);

    	mLocationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER,
    	        1000,          // 1-second interval.
    	        10,             // 10 meters.
    	        listener);
                
        mLocationStatus = (TextView)findViewById(R.id.locationstatus);
    }
    
    @Override
    public void finishFromChild(Activity child) 
    {
    	Refresh(api.getCachedMsqs());
    };
    
    public void Refresh(View btn)
    {
    	Refresh(api.getNearbyMsqs(mCurrentLocation));
    }
    
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) 
    {
    	Refresh(api.getCachedMsqs());
    };

    public void Post(View btn)
    {
    	Intent intent = new Intent(this, MsqDetailsActivity.class);
    	startActivityForResult(intent, 0);
    }
    
    public void Refresh(List<MsqModel> msqs)
    {
    	if (mCurrentLocation == null)
    	{
        	Toast.makeText(this, "Still waiting for location", Toast.LENGTH_LONG).show();
    		return;
    	}
    	        
        String[] from = new String[] {"message", "position"};
        int[] to = new int[] { android.R.id.text1, android.R.id.text2 };

        // prepare the list of all records
        List<HashMap<String, String>> fillMaps = new ArrayList<HashMap<String, String>>();
        for(MsqModel msq: msqs)
        {
        	HashMap<String, String> map = new HashMap<String, String>();
        	map.put("message", msq.getMessage());
        	map.put("position", String.format("%.2f km", Double.valueOf(msq.getLocation().distanceTo(mCurrentLocation) / 1000.0)));
        	fillMaps.add(map);
        }
                
        ListView lv = (ListView)findViewById(R.id.mainListView);
        
        SimpleAdapter adapter = new SimpleAdapter(
        		this, 
        		fillMaps, 
        		android.R.layout.simple_list_item_2, 
        		from, 
        		to);
        
        lv.setAdapter(adapter);
    }

//    @Override
//    public boolean onCreateOptionsMenu(Menu menu) {
//        getMenuInflater().inflate(R.menu.activity_main, menu);
//        return true;
//    }
    
    protected void onStop() {
        super.onStop();
        mLocationManager.removeUpdates(listener);
    }

	public void onTabSelected(Tab tab, FragmentTransaction ft) {
		// TODO Auto-generated method stub
		
	}

	public void onTabUnselected(Tab tab, FragmentTransaction ft) {
		// TODO Auto-generated method stub
		
	}

	public void onTabReselected(Tab tab, FragmentTransaction ft) {
		// TODO Auto-generated method stub
		
	}
}


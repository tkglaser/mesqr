package com.mesqr.android.db;

// THIS CODE IS GENERATED BY greenDAO, DO NOT EDIT. Enable "keep" sections if you want to edit. 
/**
 * Entity mapped to table TABLE.
 */
public class Table {

    private Long id;
    private String Name;
    private Double Latitude;
    private Double Longitude;
    private Double TableRadius;
    private String RowGuid;
    /** Not-null value. */
    private java.util.Date Entered;

    public Table() {
    }

    public Table(Long id) {
        this.id = id;
    }

    public Table(Long id, String Name, Double Latitude, Double Longitude, Double TableRadius, String RowGuid, java.util.Date Entered) {
        this.id = id;
        this.Name = Name;
        this.Latitude = Latitude;
        this.Longitude = Longitude;
        this.TableRadius = TableRadius;
        this.RowGuid = RowGuid;
        this.Entered = Entered;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getName() {
        return Name;
    }

    public void setName(String Name) {
        this.Name = Name;
    }

    public Double getLatitude() {
        return Latitude;
    }

    public void setLatitude(Double Latitude) {
        this.Latitude = Latitude;
    }

    public Double getLongitude() {
        return Longitude;
    }

    public void setLongitude(Double Longitude) {
        this.Longitude = Longitude;
    }

    public Double getTableRadius() {
        return TableRadius;
    }

    public void setTableRadius(Double TableRadius) {
        this.TableRadius = TableRadius;
    }

    public String getRowGuid() {
        return RowGuid;
    }

    public void setRowGuid(String RowGuid) {
        this.RowGuid = RowGuid;
    }

    /** Not-null value. */
    public java.util.Date getEntered() {
        return Entered;
    }

    /** Not-null value; ensure this value is available before it is saved to the database. */
    public void setEntered(java.util.Date Entered) {
        this.Entered = Entered;
    }

}

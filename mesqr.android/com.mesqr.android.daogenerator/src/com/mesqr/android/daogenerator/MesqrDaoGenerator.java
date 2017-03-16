package com.mesqr.android.daogenerator;

import de.greenrobot.daogenerator.DaoGenerator;
import de.greenrobot.daogenerator.Entity;
import de.greenrobot.daogenerator.Property;
import de.greenrobot.daogenerator.Schema;

public class MesqrDaoGenerator {

	public static void main(String[] args) throws Exception {
		Schema schema = new Schema(2, "com.mesqr.android.db");
		
		Entity table = schema.addEntity("Table");
		table.addIdProperty();
		table.addStringProperty("Name");
		table.addDoubleProperty("Latitude");
		table.addDoubleProperty("Longitude");
		table.addDoubleProperty("TableRadius");
		table.addStringProperty("RowGuid");
		table.addDateProperty("Entered").notNull();

		Entity msq = schema.addEntity("Msq");
		msq.addIdProperty();
		msq.addStringProperty("Message");
		msq.addStringProperty("FriendlyPosition");
		msq.addStringProperty("UserName");
		msq.addDoubleProperty("Latitude").notNull();
		msq.addDoubleProperty("Longitude").notNull();
		msq.addDoubleProperty("Accuracy").notNull();
		msq.addDoubleProperty("Altitude");
		msq.addStringProperty("RowGuid");
		msq.addDateProperty("Entered").notNull();
		
		Property tableLink = msq.addLongProperty("TableId").getProperty();
		msq.addToOne(table, tableLink);
		
		new DaoGenerator().generateAll(schema, "../MeSqr/src-gen");
	}
	
}

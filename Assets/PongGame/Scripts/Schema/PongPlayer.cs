// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

public partial class PongPlayer : Schema {
	[Type(0, "string")]
	public string id = default(string);

	[Type(1, "number")]
	public float pos = default(float);

	[Type(2, "boolean")]
	public bool ai = default(bool);
}


// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.35
// 

using Colyseus.Schema;
using Action = System.Action;
#if UNITY_5_3_OR_NEWER
using UnityEngine.Scripting;
#endif

public partial class PongPlayer : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public PongPlayer() { }
	[Type(0, "string")]
	public string id = default(string);

	[Type(1, "number")]
	public float pos = default(float);

	[Type(2, "boolean")]
	public bool ai = default(bool);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<string> __idChange;
	public Action OnIdChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.id));
		__idChange += __handler;
		if (__immediate && this.id != default(string)) { __handler(this.id, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(id));
			__idChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<float> __posChange;
	public Action OnPosChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.pos));
		__posChange += __handler;
		if (__immediate && this.pos != default(float)) { __handler(this.pos, default(float)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(pos));
			__posChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<bool> __aiChange;
	public Action OnAiChange(PropertyChangeHandler<bool> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.ai));
		__aiChange += __handler;
		if (__immediate && this.ai != default(bool)) { __handler(this.ai, default(bool)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(ai));
			__aiChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(id): __idChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(pos): __posChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
			case nameof(ai): __aiChange?.Invoke((bool) change.Value, (bool) change.PreviousValue); break;
			default: break;
		}
	}
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observable<T>
{
	private T value;

	private T lastValue;

	public Action<Observable<T>, T, T> OnChanged;

	public T Value
	{
		get { return value; }
		set
		{
			this.value = value;
			if (!lastValue.Equals(value))
			{
				OnChanged?.Invoke(this, lastValue, value);
				lastValue = value;
			}
        }
	}
}

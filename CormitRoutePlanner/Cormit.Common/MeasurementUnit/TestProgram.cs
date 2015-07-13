#if DEBUG
using System;
using Imarda.Lib;
using System.Globalization;
using System.Collections.Specialized;
using System.Collections.Generic;

public class Program
{
	static void Main(string[] args)
	{
		Console.WindowHeight = 60;
		var ci = new CultureInfo("en-NZ");
		var mfi = new MeasurementFormatInfo(ci.NumberFormat);
		var dict = new HybridDictionary();
		dict["FuelEfficiency"] = "km/L;0.0";
		dict["Length"] = "in";
		dict["MassFlow"] = "t/d;0.00000";
		dict["Angle"] = ">+0d0'0.00\"";
		dict["Angle~brg"] = "<{>B} ({>+000d})";
		dict["Angle~test"] = "<{~brg} {/3.1415926536;0.00}\u03C0 {!;0.0} {*} {%} {>x}";

		mfi.SetPreferences(dict);

		var ang1 = Angle.Degrees(-25);
		string sAng1 = ang1.ToString("~test", mfi);
		Console.WriteLine("composite format: 25deg: {0}", sAng1);
		var ang2 = Angle.Degrees(333);
		string sAng2 = ang2.ToString("~brg", mfi);
		Console.WriteLine("composite format: 333deg:  {0} {1:>X}", sAng2, ang2);

		for (int deg3 = 0; deg3 < 700; deg3 += 50)
		{
			Angle ang3 = Angle.Degrees(deg3);
			string sAng3 = ang3.ToString("<{!},{>0d},{>+0d},{>-000d00'00.0\"}", mfi);
			Console.WriteLine("normal, full and half: {0}", sAng3);
		}

		string x1 = string.Format(mfi,
				"\r\nUnitless:	{0:*}" + // unitless SI value
				"\r\nKnown unit:  {1:km/L}" + // use km/L unit
				"\r\nMultiplier:  {2:*3.6;0,000.000}" + // unitless, multiply SI value
				"\r\nCanonical:   {3:!;0.00}" + // show simple SI base units with dots between units
				"\r\nSI base:	 {4:^}" + // show simple SI base units without dots between, and using superscripts for 1..3
				"\r\nAngle:	   {5:deg;0.0}" + // display unit different from the lookup key
				"\r\nCurrency:	{6:@}" + // @ is the lookup character for the generic currency symbol
				"\r\nTemperature: {7:degF;0.0}" +  // temperature scale conversion
				"\r\nFeet:		{8:';30.0}" + // use of ' mark
				"\r\nPreferred:   {9}" + // uses the preferences in MeasureFormatInfo
				"\r\nEasy unit:   {10: m3}" + // converts 3 to superscript before lookup
				"\r\nSlash unit:  {11:ps;0}" // 'per' ...

				, Length.Metre(10) //0
				, FuelEfficiency.KmPerLitre(11.5) //1
				, Speed.MetrePerSec(893483.0) //2
				, Pressure.Pascal(1013.293840129348) //3
				, Volume.CubicMetre(7.0012) //4
				, Angle.Degrees(89.55) //5
				, new Currency(1200) //6
				, Temperature.Kelvin(233.15) //7
				, Length.Metre(3) //8
				, MassFlow.KgPerSec(2.3) // 9
				, Volume.USGallon(100.0) // 10
				, Frequency.Hertz(60) // 11
		);

		var m1 = new Measurement(0.1334, new MUnit(2, 3, -1, 0, 0, 1, 0));
		string x2 = string.Format(mfi,
				"\r\nCanonical   {0:!}" +
				"\r\nSuperscript {1:^}" +
				"\r\nUnitless	{2:*}" +
				"\r\nMultiplier  {3:*100}%" +
				"\r\nDivider	 {4:/2}" +
				"\r\nPreferred   {5}"
				, m1, m1, m1, m1, m1, m1
			);
		Console.WriteLine(x1);
		Console.WriteLine(x2);
		Console.WriteLine(FuelEfficiency.USGallonPer100Mile(5.0).ToString(null, mfi));
		string q;
		Console.WriteLine(Angle.Degrees(-123.457).ToString(">000E 00' 00.0\"", mfi));
		Console.WriteLine(q = Angle.Degrees(-123.457).ToString(">#DN 00' 00.0\"", mfi));
		Console.WriteLine(Angle.Degrees(3.457).ToString(">000.00000d", mfi));
		Console.WriteLine(Angle.Degrees(-6.1234123).ToString(">0.00000d 0.00000' 0.00000\u2033", mfi));
		Console.WriteLine("---");
		var time = Time.Hours(2);
		var speed = Speed.Kph(20);
		var length = (Length)((Measurement)speed * time);
		Console.WriteLine(string.Format(mfi, "Length = {0:km}", length));
		Console.WriteLine(Length.Metre(20).ToString("yd", mfi));
		Console.WriteLine(string.Format(mfi, "-> {0}", Length.Metre(0.01)));
		Console.WriteLine("=> " + Length.Metre(0.01).ToString(null, mfi));
		//string s = FuelEfficiency.KmPerLitre(14.5).ToString("gal/100mi", measureFormatInfo);
		string s = Angle.Degrees(45).ToString("%;0.0", mfi);
		Console.WriteLine(s);

		var a = Length.Metre(5e-9);
		var ss = string.Format(mfi, "{0:/1e-9;0}nm = {1:*1e9;0}nm", a, a);
		Console.WriteLine(ss);
		ss = string.Format(mfi, "{0:*;0}nm", new Measurement(a.InSI(SIPrefix.Nano)));
		Console.WriteLine(ss);


		var fe1 = FuelEfficiency.KmPerLitre(11).ToString("<{km/L;0.0} ({L/100km;0})", mfi);
		Console.WriteLine("FE: " + fe1);

		var sp1 = Speed.Kph(5.1).ToString("<{*86.4;0}km/24h", mfi);
		Console.WriteLine("Daily distance " + sp1);



		string z = "Length:12";
		Length l1 = (Length)Measurement.Parse(z);
		Length l2 = Measurement.Parse<Length>(z);
		Console.WriteLine("{0}", l1);
		Console.WriteLine(l2);

		FuelEfficiency ff = FuelEfficiency.KmPerLitre(11);
		z = Measurement.PString(ff);
		Console.WriteLine(z);
		FuelEfficiency ff2 = Measurement.Parse<FuelEfficiency>(z);
		Console.WriteLine(string.Format(mfi, "FuelEfficiency, Preferred: {0}", ff2));
		Console.WriteLine(ff2.ToString(null, mfi));
		if ((Measurement)ff2 != ff) Console.WriteLine("not the same");

		var m2 = new Measurement(17.7, new MUnit(-1, -1, -3, 0, 1, -1, 2));
		Console.WriteLine(m2.ToString(null, mfi));
		z = Measurement.PString(m2);
		Console.WriteLine(z);
		var m3 = Measurement.Parse(z);
		Console.WriteLine(m3);

		var forex = mfi.ExchangeRates;
		forex["USD"] = 1.0;
		forex["NZD"] = 1.3839;
		forex["EUR"] = 0.6870;
		forex["DKK"] = 5.1121;
		var c1 = new Currency(34.98);
		var cs1 = c1.ToString("<{>USD;0 @} {>NZD;c} {>EUR;\u20AC0.00} {>DKK } {>XYZ;0} {@} {$}", mfi);
		Console.WriteLine("Currency: " + cs1);

		Console.WriteLine(new Currency(12000).ToString(">NZD ;@0;@(0);nil", mfi));

		var c2 = new Currency(36.00, "NZD", mfi);
		var cs2 = c2.ToString("<{>NZD;c} stored as {*}", mfi);
		Console.WriteLine("Conversion: "+ cs2);

		string[] af = {
										 Measurement.Format("Angle:2", null),
										 Measurement.Format("Angle:2", mfi),
										 Measurement.Format("Angle:2 ~brg", mfi),
										 Measurement.Format("Angle:2 ~test", mfi),
										 Measurement.Format("Angle:2 !;0.0", mfi),
										 Measurement.Format("Angle:2 <quadrant {>X} {>b}", mfi),
										 Measurement.Format("Angle:2 deg", mfi),
										 Measurement.Format("Currency:750 >NZD;c", mfi),
										 Measurement.Format("M:123;1,1,1,-1,-2,-2,1 <{!} ({*;0} units)", mfi),
									};
		Console.WriteLine("Measurement.Format()");
		foreach (string afi in af) Console.WriteLine("  {0}", afi);

		Console.ReadKey();
	}
}
#endif
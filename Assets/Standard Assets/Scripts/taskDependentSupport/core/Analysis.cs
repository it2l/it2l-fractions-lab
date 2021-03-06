﻿using UnityEngine;
using System.Collections;

namespace taskDependentSupport.core
{

	public class Analysis {



		public void analyseEvent(StudentModel studentModel, string type, string name, string id, string value, int fractionsValue, string position, long time)
		{
			studentModel.setEventTime(time);
			studentModel.setCurrentFraction(id);

			Debug.Log ("::: ANAYLSE: "+type+" name: "+name+" id: "+id+" value: "+value+" fractionsValue: "+fractionsValue);

			if (type.Equals ("ClickButton")){
				if (name.Equals ("Equivalence")){
					if (studentModel.getEquivalenceOpen() == 0) studentModel.setEquivalenceOpen(1);
					else {
						studentModel.setEquivalenceOpen(0);
						studentModel.setCompared(false);
						studentModel.setComparedResult(false);
						studentModel.setComparedFractions(false);
					}
				}
			}
			if (type.Equals ("FractionGenerated")){
				Fraction thisFraction = new Fraction();
				thisFraction.setName(name);
				thisFraction.setID(id);
				studentModel.addCurrentFractions(thisFraction);
				studentModel.setCompared(false);
				studentModel.setComparedResult(false);
				studentModel.setComparedFractions(false);
				studentModel.setCurrentFraction(id);

			}
			if (type.Equals("FractionChange")){
				if (name.Equals("Numerator")) studentModel.setNumeratorAtFraction(id, fractionsValue);
				if (name.Equals("Denominator")) studentModel.setDenominatorAtFraction(id, fractionsValue);
				if (name.Equals("Partitions")) studentModel.setPartitionAtFraction(id, fractionsValue);

				studentModel.setComparedResult(false);
				studentModel.setComparedResult(false);
				studentModel.setComparedFractions(false);
			}

			if (type.Equals ("FractionTrashed")){
				studentModel.removeFraction(id);
				studentModel.setCompared(false);
				studentModel.setComparedResult(false);
				studentModel.setComparedFractions(false);

			}


			if (type.Equals ("OperationResult")){
				if (name.Equals("Sum")) {
					Debug.Log (":::::: setAdditionBox ::::::");
					studentModel.setAdditionBox(true);
				}
				else if (name.Equals("Substraction")){
					studentModel.setSubstractionBox(true);
				}
				else {
					studentModel.setCompared(true);
					if (id.Equals ("=")){
						studentModel.setComparedResult(true);
					}
					else if (id.Equals (">") || id.Equals ("<")){
						studentModel.setComparedFractions(true);
					}
					else {
						studentModel.setComparedResult(false);
						studentModel.setComparedFractions(false);
					}
				}
			}

		
		}

	
	}
}

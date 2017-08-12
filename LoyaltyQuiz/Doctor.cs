using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyQuiz {
	public class Doctor {
		private string name;
		public string Name {
			get {
				return name;
			}
		}

		private string position;
		public string Position {
			get {
				return position;
			}
		}

		public Doctor(string name, string position) {
			this.name = name;
			this.position = position;
		}
	}
}

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


		private string department;
		public string Department {
			get {
				return department;
			}
		}

		private string id;
		public string Id {
			get {
				return id;
			}
		}





		public Doctor(string name, string position, string department, string id) {
			this.name = name;
			this.position = position;
			this.department = department;
			this.id = id;
		}
	}
}

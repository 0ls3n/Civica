using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class WorkTimeRepository
    {
        private List <WorkTime> _workTimeList;
       

        public WorkTimeRepository()
        {
            _workTimeList = DatabaseHelper.InitializeWorkTime();
        }
        public List<WorkTime> GetAll() => _workTimeList;

        public void Add(WorkTime workTime)
        {
            workTime.Id = DatabaseHelper.Add(workTime);
            _workTimeList.Add(workTime);
        }

        public void Update(WorkTime workTíme, double time, string involvedName)
        {
            workTime.Time = time;
            workTime.InvolvedName = involvedName;
            DatabaseHelper.Update(workTime);
        }

        public void Remove(WorkTime workTime)
        {
            DatabaseHelper.Remove(workTime);
            _workTimeList.Remove(workTime);
        }
        public WorkTime Get(int id) => _workTimeList.Find(x => x.Id == id);

        public List<WorkTime> GetByResourceId(int resourceId) => _workTimeList.FindAll(x => x.ResourceId == resourceId);
    }
}

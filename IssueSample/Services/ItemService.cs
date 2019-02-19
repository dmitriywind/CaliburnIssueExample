using System;
using System.Collections.Generic;
using System.Linq;
using IssueSample.Models;

namespace IssueSample.Services
{
    public interface IItemService
    {
        List<BaseModel> GetModels();
    }

    public class ItemService : IItemService
    {
        public List<BaseModel> GetModels()
        {
            var result = new List<BaseModel>();

            var days = 10;
            for (int i = 0; i < days; i++)
            {
                result.Add(new BaseModel
                {
                    DateTime = DateTime.Now.AddDays(-i).AddHours(-1),
                    Comment = DateTime.Now.AddDays(-i).ToString("O")
                });
                result.Add(new BaseModel
                {
                    DateTime = DateTime.Now.AddDays(-i).AddHours(-2),
                    Comment = DateTime.Now.AddDays(-i).AddDays(-i).ToString("O")
                });
                result.Add(new BaseModel
                {
                    DateTime = DateTime.Now.AddDays(-i).AddHours(-3),
                    Comment = DateTime.Now.AddDays(-i).ToString("O")
                });
            }

            var idCounter = 1;
            foreach (var baseModel in result)
            {
                baseModel.Id = idCounter;
                idCounter++;
            }

            return result.OrderByDescending(r => r.DateTime.Date).ToList();
        }
    }
}

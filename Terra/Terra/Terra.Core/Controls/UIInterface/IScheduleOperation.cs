using System;
namespace Terra.Core.Controls.UIInterface
{
    public interface IScheduleOperation
    {
        void Schedule_1_EditButtonClick(object arg, string id, TimeSpan start, TimeSpan stop, string interval, bool active);
    }
}

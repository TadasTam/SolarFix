import React from 'react';
import { useState } from 'react';
import DatePicker from 'react-datepicker';
import DayProducedEnergyChart from './DayProducedEnergyChart';


function DayProducedEnergyChartPage() {
const [selectedDate2, setSelectedDate2] = useState(new Date('2016-07-25'));
const handleSelectedDateChange2 = (date) => setSelectedDate2(date);

return (
  <div className="chart-page">
      <div className="chart-page__header">
      <label className="chart-page__label">Date: </label>
    <DatePicker className="chart-page__datepicker" selected={selectedDate2} onChange={handleSelectedDateChange2} />
    </div>
    <div className="chart-page__chart">
    <DayProducedEnergyChart date={selectedDate2} />
  </div>
</div>
);
}

export default DayProducedEnergyChartPage;
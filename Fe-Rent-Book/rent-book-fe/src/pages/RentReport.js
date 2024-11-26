import { Button, DatePicker, Table } from 'antd';
import axios from 'axios';
import React, { useState } from 'react';

const RentReport = () => {
  const [reportData, setReportData] = useState([]);
  const [startDate, setStartDate] = useState(null);
  const [endDate, setEndDate] = useState(null);

  const fetchReport = async () => {
    if (startDate && endDate) {
      try {
        const response = await axios.get(`http://localhost:5289/api/v1/Rentals/report`, {
          params: {
            startDate,
            endDate
          }
        });
        setReportData(response.data);
      } catch (error) {
        console.error('Error fetching report', error);
      }
    }
  };

  return (
    <div>
      <h2>Rental Report</h2>
      <div>
        <DatePicker onChange={(date) => setStartDate(date)} placeholder="Start Date" />
        <DatePicker onChange={(date) => setEndDate(date)} placeholder="End Date" />
        <Button onClick={fetchReport}>Generate Report</Button>
      </div>
      <Table
        columns={[
          { title: 'Book Name', dataIndex: 'bookName', key: 'bookName' },
          { title: 'Rental Date', dataIndex: 'rentalDate', key: 'rentalDate' },
          { title: 'Return Date', dataIndex: 'returnDate', key: 'returnDate' },
          { title: 'Customer Name', dataIndex: 'customerName', key: 'customerName' },
          { title: 'Quantity', dataIndex: 'quantity', key: 'quantity' },
        ]}
        dataSource={reportData}
        rowKey="rentalDetailID"
      />
    </div>
  );
};

export default RentReport;

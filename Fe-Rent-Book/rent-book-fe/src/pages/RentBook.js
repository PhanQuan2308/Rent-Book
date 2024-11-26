import { Button, DatePicker, Form, Input, notification } from 'antd';
import axios from 'axios';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const RentBook = () => {
  const [form] = Form.useForm();
  const [bookList, setBookList] = useState([]);
  const navigate = useNavigate();

  const onFinish = async (values) => {
    try {
      const response = await axios.post('http://localhost:5289/api/v1/RentalDetails/create', values);
      notification.success({
        message: 'Book Rented',
        description: 'The book rental has been completed successfully.',
      });

      navigate("/report")
    } catch (error) {
      notification.error({
        message: 'Rental Failed',
        description: 'There was an error renting the book.',
      });
    }
  };

  return (
    <div>
      <h2>Rent a Book</h2>
      <Form
        form={form}
        layout="vertical"
        onFinish={onFinish}
        initialValues={{ remember: true }}
      >
        <Form.Item
          name="rentalID"
          label="Rental ID"
          rules={[{ required: true, message: 'Please input rental ID!' }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="comicBookID"
          label="Comic Book ID"
          rules={[{ required: true, message: 'Please input comic book ID!' }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="quantity"
          label="Quantity"
          rules={[{ required: true, message: 'Please input quantity!' }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="pricePerDay"
          label="Price per day"
          rules={[{ required: true, message: 'Please input price per day!' }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="rentalDate"
          label="Rental Date"
          rules={[{ required: true, message: 'Please select rental date!' }]}
        >
          <DatePicker />
        </Form.Item>
        <Form.Item
          name="returnDate"
          label="Return Date"
          rules={[{ required: true, message: 'Please select return date!' }]}
        >
          <DatePicker />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit">
            Rent Book
          </Button>
        </Form.Item>
      </Form>
    </div>
  );
};

export default RentBook;

import { Button, DatePicker, Form, Input, notification } from "antd";
import axios from "axios";
import React from "react";
import { useNavigate } from "react-router-dom";

const CustomerRegistration = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();

  const onFinish = async (values) => {
    try {
      const response = await axios.post(
        "http://localhost:5289/api/v1/Customers/create",
        values
      );
      
      notification.success({
        message: "Customer Registered",
        description: "The customer has been registered successfully.",
      });
      navigate("/rent")
    } catch (error) {
      notification.error({
        message: "Registration Failed",
        description: "There was an error registering the customer.",
      });
    }
  };

  return (
    <div>
      <h2>Customer Registration</h2>
      <Form
        form={form}
        layout="vertical"
        onFinish={onFinish}
        initialValues={{ remember: true }}
      >
        <Form.Item
          name="fullName"
          label="Full Name"
          rules={[{ required: true, message: "Please input your full name!" }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="phoneNumber"
          label="Phone Number"
          rules={[
            { required: true, message: "Please input your phone number!" },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="registrationDate"
          label="Registration Date"
          rules={[
            { required: true, message: "Please input your registration date!" },
          ]}
        >
          <DatePicker />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit">
            Register
          </Button>
        </Form.Item>
      </Form>
    </div>
  );
};

export default CustomerRegistration;

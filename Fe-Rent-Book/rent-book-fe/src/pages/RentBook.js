import { Button, DatePicker, Form, Input, notification, Select } from "antd";
import axios from "axios";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const RentBook = () => {
  const [form] = Form.useForm();
  const [comicBooks, setComicBooks] = useState([]);
  const navigate = useNavigate();

  // Lấy customerID từ localStorage
  const customerID = localStorage.getItem("customerID");

  useEffect(() => {
    const fetchComicBooks = async () => {
      try {
        const response = await axios.get(
          "http://localhost:5289/api/v1/ComicBooks"
        );
        setComicBooks(response.data);
      } catch (error) {
        console.error("Error fetching comic books:", error);
      }
    };
    fetchComicBooks();
  }, []);

  const onFinish = async (values) => {
    try {
      // Gửi yêu cầu tạo rental (thuê sách)
      const rentalResponse = await axios.post(
        "http://localhost:5289/api/v1/Rentals/create",
        {
          customerID: customerID,  // Lấy customerID từ localStorage
          rentalDate: values.rentalDate,
          returnDate: values.returnDate,
          status: "Ongoing",
        }
      );

      const rentalID = rentalResponse.data.rentalID;  // Lấy rentalID từ response

      // Gửi yêu cầu tạo rentalDetail (chi tiết thuê sách)
      const rentalDetailResponse = await axios.post(
        "http://localhost:5289/api/v1/RentalDetails/create",
        {
          rentalID: rentalID,       // rentalID từ response trước
          comicBookID: values.comicBookID,  // comicBookID từ select
          quantity: values.quantity,        // Số lượng sách
          pricePerDay: values.pricePerDay,  // Giá thuê theo ngày
        }
      );

      notification.success({
        message: "Book Rented",
        description: "The book rental has been completed successfully.",
      });

      navigate("/report");

    } catch (error) {
      notification.error({
        message: "Rental Failed",
        description: "There was an error renting the book.",
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
          name="comicBookID"
          label="Comic Book"
          rules={[{ required: true, message: "Please select a comic book!" }]}
        >
          <Select placeholder="Select a comic book">
            {comicBooks.map((book) => (
              <Select.Option key={book.comicBookID} value={book.comicBookID}>
                {book.title}  {/* Hiển thị title thay vì ID */}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>
        <Form.Item
          name="quantity"
          label="Quantity"
          rules={[{ required: true, message: "Please input quantity!" }]}
        >
          <Input type="number" />
        </Form.Item>
        <Form.Item
          name="pricePerDay"
          label="Price per day"
          rules={[{ required: true, message: "Please input price per day!" }]}
        >
          <Input type="number" />
        </Form.Item>
        <Form.Item
          name="rentalDate"
          label="Rental Date"
          rules={[{ required: true, message: "Please select rental date!" }]}
        >
          <DatePicker />
        </Form.Item>
        <Form.Item
          name="returnDate"
          label="Return Date"
          rules={[{ required: true, message: "Please select return date!" }]}
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

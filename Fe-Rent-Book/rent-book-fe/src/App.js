import { Layout } from "antd";
import React from "react";
import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
import CustomerRegistration from "./pages/CustomerRegistration";
import RentBook from "./pages/RentBook";
import RentReport from "./pages/RentReport";

const { Header, Content, Footer } = Layout;

function App() {
  return (
    <Router>
      <Layout>
        <Header className="header">
          <div className="logo" />
        </Header>
        <Content style={{ padding: "0 50px", marginTop: 64 }}>
          <Routes>
            <Route path="/" element={<CustomerRegistration />} />
            <Route path="/rent" element={<RentBook />} />
            <Route path="/report" element={<RentReport />} />
          </Routes>
        </Content>
        <Footer style={{ textAlign: "center" }}>Rental App ©2024</Footer>
      </Layout>
    </Router>
  );
}

export default App;

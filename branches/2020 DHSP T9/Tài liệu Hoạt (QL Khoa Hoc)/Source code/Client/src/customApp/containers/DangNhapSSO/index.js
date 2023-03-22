import React from "react";
import { Spin } from "antd";
import { connect } from "react-redux";
import logoHNUE from "../../../image/logoHNUE.png";
import "./style.css";
import querystring from "query-string";
import api from "./config";
import { isFullLocalStorage } from "../../../helpers/utility";
import { Link, Redirect } from "react-router-dom";
import actions from "../../../redux/auth/actions";

class DangNhapSSO extends React.Component {
  componentDidMount() {
    const ssoData = querystring.parse(this.props.location.search);
    this.props.loginSSO({ AccessToken: this.props.location.search });

    // api.DangNhapSSO({ AccessToken: ssoData.accessToken }).then((res) => {
    //   console.log(res);
    // });
  }
  render() {
    const localStorageNotNull = isFullLocalStorage();
    if (localStorageNotNull) {
      return <Redirect to={"/dashboard"} />;
    }
    return (
      <div className="isoSignInPage">
        <div className={"title-login"}>
          <img src={logoHNUE} alt={"logo"} className={"title-logo"} />
          <div className={"title-small"}>Công ty Cổ phần Hệ thống 2B</div>
          2BS 12 năm xây dựng & phát triển 1
        </div>
        <div className={"main-login"}>
          <div className={"main-title"}>Đăng nhập hệ thống</div>
          <div className={"main-body "}>
            <Spin tip="Đang xác thực người dùng..."></Spin>
          </div>
        </div>
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    ...state.DangNhapSSO,
  };
}

export default connect(mapStateToProps, actions)(DangNhapSSO);

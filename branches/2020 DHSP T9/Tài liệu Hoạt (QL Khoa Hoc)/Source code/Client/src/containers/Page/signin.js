import React, { Component } from "react";
import { Link, Redirect } from "react-router-dom";
import { connect } from "react-redux";
import Button from "../../components/uielements/button";
import authAction from "../../redux/auth/actions";
import appAction from "../../redux/app/actions";
import { Row, Col, Icon, Tooltip, Modal, Input } from "antd";
import logoHNUE from "../../image/logoHNUE.png";
import { isFullLocalStorage } from "../../helpers/utility";
import "./style.css";
import api from "./config";
import apiSystemConfig from "../../customApp/containers/ThamSoHeThong/config";

const { login } = authAction;
const { clearMenu } = appAction;
const date = new Date();
const currentYear = date.getFullYear();
let replaceString = "";
class SignIn extends Component {
  constructor(props) {
    // console.log(window.location);

    document.title = "Đăng nhập hệ thống";
    super(props);
    this.state = {
      confirmLoading: false,
      username: "",
      password: "",
      messageError: "",
      info: {
        Email: "",
        Phone: "",
        Fax: "",
        Website: "",
        Support: "",
      },
      selected: "ADMIN",
    };
    this.changeRoles = this.changeRoles.bind(this);
  }

  //Get initData---------------------------------------------
  componentDidMount = () => {
    api.getDataConfig({ ConfigKey: "LOGIN_URL" }).then((res) => {
      console.log("res login url ", res);
      replaceString = `${res.data.Data.ConfigValue}%2Foauth%2FAccess%3FreturnUrl%3D${window.location.host.includes("http://") ? "" : "http://"}${window.location.host}/auth/sso`;
      // replaceString = `http://192.168.100.15:3000/auth/sso?`;
      //DUOWNGTORAFAKETOKEN

    });
    api.getDataConfig({ ConfigKey: "Thong_Tin_Ho_Tro" }).then((response) => {
      if (response && response.data.Status > 0) {
        const data = response.data.Data.ConfigValue;
        const dataArr = data.split(";");
        const info = {
          Email: dataArr[0] ? dataArr[0] : "",
          Phone: dataArr[1] ? dataArr[1] : "",
          Fax: dataArr[2] ? dataArr[2] : "",
          Website: dataArr[3] ? dataArr[3] : "",
          Support: dataArr[4] ? dataArr[4] : "",
        };
        this.setState({ info });
      }
    });
  };

  handleLogin = () => {
    this.setState({ confirmLoading: true }, () => {
      setTimeout(() => {
        const username = this.state.username;
        const password = this.state.password;
        //check api
        if (username && password) {
          const data = {
            UserName: username,
            Password: password,
          };
          api
            .dangNhap(data)
            .then((response) => {
              if (response.data.Status > 0) {
                this.setState(
                  {
                    confirmLoading: false,
                    username: "",
                    password: "",
                    messageError: "",
                  },
                  () => {
                    const { login, clearMenu } = this.props;
                    login(data);
                    clearMenu();
                  }
                );
              } else {
                this.setState(
                  {
                    confirmLoading: false,
                    messageError: response.data.Message,
                  },
                  () => {
                    setTimeout(() => {
                      this.setState({
                        messageError: "",
                      });
                    }, 5000);
                  }
                );
              }
            })
            .catch((error) => {
              this.systemError();
            });
        } else {
          this.setState({
            confirmLoading: false,
            messageError: "Vui lòng nhập đầy đủ thông tin!",
          });
        }
      }, 500);
    });
  };
  setUsername = (value) => {
    this.setState({ username: value, messageError: "" });
  };
  setPassword = (value) => {
    this.setState({ password: value, messageError: "" });
  };
  _handleKeyDown = (e) => {
    if (e.key === "Enter") {
      this.handleLogin();
    }
  };
  systemError = () => {
    this.setState({
      confirmLoading: false,
    });
    Modal.error({
      title: "Không thể đăng nhập",
      content: "Đã có lỗi xảy ra ...",
    });
  };
  changeRoles (event){
    console.log('event', event.target.value);
    this.setState({ selected: event.target.value });
  }
  render() {
    const { messageError, username, password, confirmLoading, info } = this.state;

    const from = { pathname: "/dashboard" };
    //reduxStorage data -> this.props.reducerToken
    const reduxStorageNotNull = this.props.reducerToken !== null;
    const localStorageNotNull = isFullLocalStorage();
    const isLoggedIn = reduxStorageNotNull || localStorageNotNull;
    if (isLoggedIn) {
      return <Redirect to={from} />;
    } else {
      localStorage.clear();
    }
    return (
      <div className="isoSignInPage row justify-content-start">
        <div className={"title-login  "}>
          <img src={logoHNUE} alt={"logo"} className={"title-logo"} />
          <div className={"title-small my-3"}>Công ty Cổ phần Hệ thống 2B</div>
          <div className="my-3">2BS 12 năm xây dựng & phát triển</div>
          <div className="my-3" style={{color:"black"}}>
            <select value={this.state.selected} name="selected_role" onChange={this.changeRoles}>
              <option value="ADMIN">Quản trị hệ thống</option>
              <option value="QLKH">Quản lý</option>
              <option value="NKH">Nhà khoa học</option>
            </select>
          </div>
          <Button
            className="my-5"
            type="primary"
            size="large"
            onClick={() => {
              window.location.replace(replaceString + this.state.selected);
            }}
          >
            Đăng nhập
          </Button>
        </div>
      </div>
    );
  }
}
{
  /* <div className="isoSignInPage"> */
}
{
  /* <div className={"title-login"}>
          <img src={logoHNUE} alt={"logo"} className={"title-logo"} />
          <div className={"title-small"}>TRƯỜNG ĐẠI HỌC SƯ PHẠM HÀ NỘI</div>
          PHẦN MỀM QUẢN LÝ KHOA HỌC VÀ CÔNG NGHỆ
        </div>
        <div className={"main-login"}>
          <div className={"main-title"}>Đăng nhập hệ thống</div>
          <div className={"main-body"}>
            <div className={"login-input"}>
              <div className={"row-title row-long"}>Tên đăng nhập / Email (nếu đã khai báo)</div>
              <div>
                <Input value={username} placeholder="Tên đăng nhập" onChange={(input) => this.setUsername(input.target.value)} onKeyDown={this._handleKeyDown} className={"txtLogin"} />
              </div>
              <div className={"row-title"}>Mật khẩu</div>
              <div>
                <Input.Password value={password} placeholder="Mật khẩu" onChange={(input) => this.setPassword(input.target.value)} onKeyDown={this._handleKeyDown} className={"txtLogin"} />
              </div>
              <div className={"btnLogin"}>
                <div className={"main-error"}>{messageError}</div>
                <Button type={"primary"} onClick={this.handleLogin} loading={confirmLoading}>
                  Đăng nhập {!confirmLoading ? <Icon type={"login"} /> : ""}
                </Button>
              </div>
            </div>
            <div className={"info-login"}>
              @{currentYear} Trường đại học sư phạm Hà Nội
              <br />
              Email: {info.Email} - Phone: {info.Phone} - Fax: {info.Fax}
              <br />
              Website: {info.Website}
              <br />
              Hỗ trợ: {info.Support}
            </div>
          </div>
        </div> */
}
export default connect(
  (state) => ({
    reducerToken: state.Auth.idToken,
    //da dang nhap khi co reduce idToken hoac co localStore
  }),
  { login, clearMenu }
)(SignIn);

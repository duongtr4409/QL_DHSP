import React, { Component } from 'react';
import { connect } from 'react-redux';
import Popover from '../../components/uielements/popover';
import IntlMessages from '../../components/utility/intlMessages';
import defaultAvatar from '../../image/defaultAvatar.jpeg';
import authAction from '../../redux/auth/actions';
import TopbarDropdownWrapper from './topbarDropdown.style';
import {Link} from "react-router-dom";
import {store} from '../../redux/store';
import {ModalChangePassword} from "./modalChangePassword";

const { logout } = authAction;

class TopbarUser extends Component {
  constructor(props) {
    super(props);
    this.handleVisibleChange = this.handleVisibleChange.bind(this);
    this.hide = this.hide.bind(this);
    this.state = {
      visible: false,
      visibleModalChangePassword: false,
      modalKey: 0
    };
  }
  hide() {
    this.setState({ visible: false });
  }
  handleVisibleChange() {
    this.setState({ visible: !this.state.visible });
  }
  renderUser = () => {
    const userAuth = store.getState().Auth.user;
    let Ten = userAuth ? userAuth.TenNguoiDung : "";
    let Avatar = userAuth ? userAuth.AnhHoSo : null;
    if(!Ten){ //neu redux store chua san sang, lay data tu local store
      const userJson = localStorage.getItem('user');
      if(userJson){
        const user = JSON.parse(userJson);
        Ten = user.TenNguoiDung ? user.TenNguoiDung : "";
        Avatar = user.AnhHoSo ? user.AnhHoSo : null;
      }
    }
    if(Ten.length > 15){
      Ten = Ten.substr(0, 10) + '...';
    }
    return {
      name: Ten,
      avatar: Avatar ? Avatar : defaultAvatar
    };
  };

  showModalChangePassword = () => {
    let modalKey = this.state.modalKey + 1;
    this.setState({
      visibleModalChangePassword: true,
      visible: false,
      modalKey: modalKey
    })
  };
  hideModalChangePassword = () => {
    this.setState({visibleModalChangePassword: false});
  };


  render() {
    const content = (
      <TopbarDropdownWrapper className="isoUserDropdown">
        {/*<a className="isoDropdownLink">*/}
        {/*  Chỉnh sửa hồ sơ*/}
        {/*</a>*/}
        <a className="isoDropdownLink" onClick={this.showModalChangePassword}>
          Đổi mật khẩu
        </a>
        <ModalChangePassword
          visible={this.state.visibleModalChangePassword}
          onCancel={this.hideModalChangePassword}
          logout={this.props.logout}
          key={this.state.modalKey}
        />
        <a className="isoDropdownLink" onClick={this.props.logout}>
          Đăng xuất
        </a>
      </TopbarDropdownWrapper>
    );
    const renderUser = this.renderUser();

    return (
      <Popover
        content={content}
        trigger="click"
        visible={this.state.visible}
        onVisibleChange={this.handleVisibleChange}
        arrowPointAtCenter={true}
        placement="bottomLeft"
      >
        <div style={{display: "flex"}}>
          <div className="isoImgWrapper" style={{display: "inline-flex"}}>
            <img alt="user" src={renderUser.avatar} style={{borderRadius: "50%", width: '100%', height: '100%'}}/>
            <span className="userActivity online" />
          </div>
          <div style={{padding: "7px 0 0 5px"}}>
            <span style={{color: "white"}}>{renderUser.name}</span>
          </div>
        </div>
      </Popover>
    );
  }
}
export default connect(null, { logout })(TopbarUser);

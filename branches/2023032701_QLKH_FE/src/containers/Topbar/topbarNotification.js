import React, { Component } from "react";
import { message, Modal, Popover, Col, Empty } from "antd";
import { connect } from "react-redux";
import TopbarDropdownWrapper from "./topbarDropdown.style";
import { getToken } from "../../helpers/utility";
import api from "./config";
import actions from "../../customApp/redux/NhacViec/actions";
import defaultAvatar from "../../image/defaultAvatar.jpeg";

class TopbarNotification extends Component {
  constructor(props) {
    super(props);
    this.handleVisibleChange = this.handleVisibleChange.bind(this);
    this.hide = this.hide.bind(this);
    this.state = {
      visible: false,
      filterData: {
        PageSize: 2,
        PageNumber: 1,
      },
      XemThem: false,
      DaXemTatCa: false,
      ListNotification: [],
      isTouchNoti: false,
      LocChuaXem: false,
    };
  }

  componentDidMount() {
    const userJson = localStorage.getItem("user");
    if (userJson) {
      // this.props.getNotifications(this.state.filterData);
      // setInterval(() => {
      //   const userId = getToken().get('userId');
      //   if (userId) {
      //     this.props.getNotifications(this.state.filterData);
      //   }
      // }, 1000 * 60 * 30); //1.800.000ms = 0.5h
    }
  }

  hide() {
    this.setState({ visible: false });
  }

  handleVisibleChange() {
    this.setState({ visible: !this.state.visible });
  }

  openNotification = (ThongBaoID, index) => {
    // api.updateNotifications({ThongBaoID: ThongBaoID, LoaiUpdate: 1})
    //   .then(response => {
    //     if (response.data.Status > 0) {
    //       const {notifications} = this.props;
    //       const {isTouchNoti} = this.state;
    //       const ListNotificationState = this.state.ListNotification;
    //       let ListNotification = isTouchNoti ? ListNotificationState.concat(notifications) : notifications;
    //       ListNotification[index].TrangThaiXem = 0;
    //       this.setState({visible: !this.state.visible, ListNotification, isTouchNoti: true});
    //     } else {
    //       message.destroy();
    //       message.error(response.data.Message);
    //     }
    //   }).catch(error => {
    //   message.destroy();
    //   message.error(error.toString());
    // })
  };

  XemThemNoti = () => {
    let { filterData, ListNotification } = this.state;
    ListNotification = ListNotification.concat(this.props.notifications);
    filterData.PageNumber++;
    this.setState({ filterData, isTouchNoti: true, ListNotification }, () => {
      this.props.getNotifications(filterData);
    });
  };

  CheckAllReaded = () => {
    api
      .updateNotifications({ LoaiUpdate: 2 })
      .then((response) => {
        if (response.data.Status > 0) {
          this.setState({ DaXemTatCa: true });
        } else {
          message.destroy();
          message.error(response.data.Message);
        }
      })
      .catch((error) => {
        message.destroy();
        message.error(error.toString());
      });
  };

  render() {
    const { customizedTheme, notifications, TableLoading, TotalRow } = this.props;
    // const {filterData, DaXemTatCa, isTouchNoti, LocChuaXem} = this.state;
    // const ListNotificationState = this.state.ListNotification;
    // let ListNotification = isTouchNoti ? ListNotificationState.concat(notifications) : notifications;
    // if (LocChuaXem) {
    //   ListNotification = ListNotification.filter(item => item.TrangThaiXem > 0)
    // }
    //
    const content = (
      // <TopbarDropdownWrapper className="topbarNotification">
      //   <div className="isoDropdownHeader">
      //     <h3>THÔNG BÁO</h3>
      //   </div>
      //   {ListNotification.length > 0 ? <div style={{padding: 5}}>
      //     <Col span={12}>
      //       <a onClick={() => this.setState({LocChuaXem: true, isTouchNoti: true})}>Lọc thông báo chưa xem</a>
      //     </Col>
      //     <Col span={12} style={{textAlign: 'right'}}>
      //       <a onClick={() => this.CheckAllReaded}>Đánh dấu tất cả đã đọc</a>
      //     </Col>
      //   </div> : ""}
      //   <div className="isoDropdownBody">
      //     {
      //       ListNotification && ListNotification.length
      //         ? (
      //           <div>
      //             {ListNotification.map((notification, index) => (
      //               <a className="isoDropdownListItem"
      //                  href={`/dashboard/${notification.Key}`}
      //                  key={index}
      //                  onClick={this.openNotification(notification.ThongBaoID, index)}
      //                  style={{backgroundColor: notification.TrangThaiXem > 0 && !DaXemTatCa ? '#d6d6d6' : ''}}>
      //                 <div style={{display: "flex"}}>
      //                   <div>
      //                     <img src={notification.AnhHoSo !== "" ? notification.AnhHoSo : defaultAvatar} alt=""
      //                          style={{height: 35, width: 35, borderRadius: "50%", marginRight: 20}}/>
      //                   </div>
      //                   <div>
      //                     <h5>{notification.Name}</h5>
      //                     <div style={{fontSize: 13, color: "grey", textAlign: 'right'}}>{notification.Time}</div>
      //                   </div>
      //                 </div>
      //                 {/*<p>{notification.notification}</p>*/}
      //               </a>
      //             ))}
      //           </div>
      //         )
      //         : <div style={{padding: 10, textAlign: "center", color: "grey"}}>Không có thông báo nhắc việc</div>
      //     }
      //   </div>
      //   {ListNotification.length < TotalRow ? <div style={{textAlign: 'center'}}>
      //     <a onClick={this.XemThemNoti}>{'Xem thêm'}</a></div> : ""}
      // </TopbarDropdownWrapper>
      <Empty description={"Không có dữ liệu"} />
    );
    return (
      <Popover content={content} trigger="click" visible={this.state.visible} onVisibleChange={this.handleVisibleChange} placement="bottomLeft" overlayClassName={"test-notification-overlay-classname"}>
        <div className="isoIconWrapper" style={{ marginTop: 4 }}>
          <i className="ion-android-notifications" style={{ color: customizedTheme.textColor }} />
          {/*{*/}
          {/*  ListNotification && ListNotification.length > 0*/}
          {/*    ? <span*/}
          {/*      style={{background: 'crimson'}}>{ListNotification.filter(item => item.TrangThaiXem > 0).length}</span>*/}
          {/*    : null*/}
          {/*}*/}
        </div>
      </Popover>
    );
  }
}

// function mapStateToProps(state) {
//   return {
//     ...state.NhacViec
//   };
// }

export default connect(
  (state) => ({
    ...state.NhacViec,
    customizedTheme: state.ThemeSwitcher.topbarTheme,
  }),
  actions
)(TopbarNotification);

// export default connect(
//   mapStateToProps,
//   actions
// )(TopbarNotification);

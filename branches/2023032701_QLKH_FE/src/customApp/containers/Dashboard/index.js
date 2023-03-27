import React, {Component} from "react";
import {connect} from "react-redux";
import actions from "../../redux/Dashboard/actions";
import './style.css';
import appActions from "../../../redux/app/actions";
import Redirect from "react-router/Redirect";
import apiConfig from '../ThamSoHeThong/config';


const {changeCurrent} = appActions;

class Dashboard extends Component {
  constructor(props) {
    super(props);
    document.title = "Quản lý khoa học";
    this.state = {
      roleNKH: null,
      roleQLNKH: null,
      roleTP: null
    };
  }

  async componentDidMount() {
    const dataNKH = await apiConfig.GetByKey({ConfigKey: 'ID_NHOM_QUYEN_NKH'});
    const dataQLNKH = await apiConfig.GetByKey({ConfigKey: 'ID_NHOM_QUYEN_QLKH'});
    const dataTP = await apiConfig.GetByKey({ConfigKey: 'ID_NHOM_QUYEN_TRUONG_KHOA'});
    const roleNKH = dataNKH.data.Data.ConfigValue;
    const roleQLNKH = dataQLNKH.data.Data.ConfigValue;
    const roleTP = dataTP.data.Data.ConfigValue;
    this.setState({roleNKH, roleQLNKH, roleTP});
    // apiConfig.GetByKey({ConfigKey: 'ID_NHOM_QUYEN_NKH'}).then(response => {
    //   if (response.data.Status > 0) {
    //     const data = response.data.Data.ConfigValue;
    //     this.setState({roleNKH: data});
    //   }
    // })
  }

  render() {
    const {roleNKH, roleQLNKH, roleTP} = this.state;
    const listRole = JSON.parse(localStorage.getItem('role'));
    const user = JSON.parse(localStorage.getItem('user'));
    const listRoleGroup = JSON.parse(localStorage.getItem('list_role'));
    const CanBoID = user.CanBoID;
    const CoQuanID = user.CoQuanID;
    const roleQLKH = listRole["ql-nha-khoa-hoc"];
    const roleCTKH = listRole["chi-tiet-nha-khoa-hoc"];
   
    console.log("role: ", listRole);
    console.log("roleQLKH: ", user);
    console.log("roleCTKH: ", listRoleGroup);
    if (roleNKH || roleQLNKH || roleTP) {
      localStorage.setItem('role_id', roleNKH);

      if (Object.values(listRole).length > 0) {
        if (roleQLKH || roleCTKH) {
          if (roleQLKH && roleQLKH.view) {
            localStorage.setItem('role_id', roleQLNKH);
            return (
              <Redirect to={`/dashboard/ql-nha-khoa-hoc`}/>
            )
          } else if (roleCTKH && roleCTKH.view) {
            return (
              <Redirect to={`/dashboard/chi-tiet-nha-khoa-hoc?CanBoID=${CanBoID}&CoQuanID=${CoQuanID}`}/>
            )
          }
        } else {
          const path = listRoleGroup[0].Role[0].MaChucNang;
          return <Redirect to={`/dashboard/${path}`}/>
        }
      }
    }
    return <div style={{width: '100%', textAlign: 'center', paddingTop: 100, color: '#999'}}>
        Tài khoản chưa được phân quyền sử dụng phần mềm
      </div>
    
  }
}

function mapStateToProps(state) {
  return {
    ...state.Dashboard
  };
}

export default connect(
  mapStateToProps,
  {...actions, changeCurrent}
)(Dashboard);

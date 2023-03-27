import React, {Component} from "react";
import Constants from "../../../settings/constants";
import {Modal, Button, message, Checkbox} from "antd";
import BoxTable from './BoxTable.style';

export class ModalDuyetThuyetMinh extends Component {
  state = {
    selectedRowKeys: 0,
    loading: false
  };

  componentDidMount() {

  }

  DuyetThuyetMinh = () => {
    const {selectedRowKeys} = this.state;
    const {onOk} = this.props;
    if (!selectedRowKeys) {
      message.destroy();
      message.warn('Chưa chọn thuyết minh duyệt');
    } else {
      Modal.confirm({
        title: 'Thông báo',
        content: 'Bạn có chắc chắn muốn duyệt thuyết minh này không ?',
        okText: 'Có',
        cancelText: 'Không',
        onOk: () => {
          onOk(selectedRowKeys);
        }
      })
    }
  };

  checkDuyetThuyetMinh = (value, ThuyetMinhID) => {
    const {selectedRowKeys} = this.state;
    const checked = value.target.checked;
    if (checked) {
      if (selectedRowKeys === ThuyetMinhID) {
        this.setState({selectedRowKeys: 0});
      } else {
        this.setState({selectedRowKeys: ThuyetMinhID})
      }
    } else {
      this.setState({selectedRowKeys: 0});
    }
  };

  renderTable = () => {
    const {selectedRowKeys} = this.state;
    const {DanhSachDuyetThuyetMinh} = this.props;
    return DanhSachDuyetThuyetMinh.map((item, index) =>
      <tr>
        <td style={{textAlign: "center", width: '5%'}}>{index + 1}</td>
        <td style={{width: '40%'}}>
          <a href={item.FileThuyetMinh.FileUrl} target={'_blank'}>{item.FileThuyetMinh.TenFileGoc}</a>
        </td>
        <td style={{width: '35%'}}>{this.renderTenCanBo(item.CanBoID, item.CoQuanID)}</td>
        <td style={{textAlign: "center", width: '20%'}}>
          <Checkbox checked={item.ThuyetMinhID === selectedRowKeys}
                    onChange={value => this.checkDuyetThuyetMinh(value, item.ThuyetMinhID)}/>
        </td>
      </tr>
    )
  };

  renderTenCanBo = (CanBoID, CoQuanID) => {
    const {DanhSachCanBo} = this.props;
    const canbo = DanhSachCanBo.filter(item => item.CanBoID === CanBoID && item.CoQuanID === CoQuanID)[0];
    if (canbo) {
      return canbo.TenCanBo;
    }
  };

  renderFooter = () => {
    const {onCancel, loading} = this.props;
    const styled = {width: '100%', textAlign: "center"};
    return <div style={styled}>
      <Button type={'primary'} loading={loading} onClick={() => this.DuyetThuyetMinh()}>Duyệt</Button>
      <Button type={'danger'} onClick={onCancel}>Hủy bỏ</Button>
    </div>
  };

  render() {
    const {visible, onCancel} = this.props;
    return (
      <Modal
        title={"Duyệt thuyết minh đề tài"}
        okText="Lưu"
        cancelText="Hủy"
        width={900}
        visible={visible}
        onCancel={onCancel}
        footer={this.renderFooter()}
      >
        <BoxTable>
          <table className='table-head'>
            <tr>
              <th style={{width: '5%'}}>STT</th>
              <th style={{width: '40%'}}>Thuyết minh đề tài</th>
              <th style={{width: '35%'}}>Chủ nhiệm đề tài</th>
              <th style={{width: '20%'}}>Quyết định duyệt</th>
            </tr>
          </table>
          <div className='scroll' style={{maxHeight: 400}}>
            <table className='table-scroll'>
              {this.renderTable()}
            </table>
          </div>
        </BoxTable>
      </Modal>
    );
  }
}

import React, {Component} from 'react';
import {Modal} from 'antd';
import BoxTable from '../../../components/utility/boxTable';


class ModalDanhSachCanBo extends Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  componentDidMount() {

  }

  render() {
    const {visible, onCancel, DanhSachCanBoDonVi} = this.props;
    const column = [
      {
        title: 'STT',
        align: 'center',
        width: '10%',
        render: (text, record, index) => (
          <span>{index + 1}</span>
        )
      },
      {
        title: 'Tên cán bộ',
        dataIndex: 'TenCanBo',
        width: '45%',
      },
      {
        title: 'Chức vụ',
        dataIndex: 'TenChucVu',
        width: '45%',
        render: (text, record) => (
          record.DanhSachTenChucVu.join(", ")
        )
      },
    ];

    return (
      <Modal
        title={"Danh sách cán bộ của đơn vị"}
        width={600}
        visible={visible}
        onCancel={onCancel}
        footer={null}
      >
        <BoxTable columns={column} dataSource={DanhSachCanBoDonVi} pagination={false} scroll={{y: 400}}/>
      </Modal>
    );
  }
}

export {ModalDanhSachCanBo}
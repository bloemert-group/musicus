import React from 'react';
import PropTypes from 'prop-types';

class Modal extends React.Component {
	render() {
		// Render nothing if the "show" prop is false
		if (!this.props.show) {
			return null;
		}

		var btnStyle = {
			margin: "10px"
		}

		return (
			<div className="overlay">
				<div className="modal">
					<div className="modal-header">
						<div className="modal-header-title">
							<div>
								{this.props.headerTitle}
							</div>
						</div>
						<div onClick={this.props.onClose}>
							<i className='icon close' style={btnStyle}></i>
						</div>
					</div>
					<div className="modal-content">
						{this.props.children}
					</div>
				</div >
			</div >
		);
	}
}

Modal.propTypes = {
	onClose: PropTypes.func.isRequired,
	show: PropTypes.bool,
	children: PropTypes.node
};

export default Modal;
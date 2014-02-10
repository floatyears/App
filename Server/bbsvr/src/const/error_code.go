package cs

//error code
const (
	SUCCESS          = 0
	FAILED           = -1
	INVALID_PARAMS   = -2
	MARSHAL_FAILED   = -3
	UNMARSHAL_FAILED = -4
)

//friend
const (
	EF_FRIEND_BASE       = -100
	EF_FRIEND_NOT_EXISTS = EF_FRIEND_BASE - 1
	EF_GET_USERINFO_FAIL = EF_FRIEND_BASE - 2
)
